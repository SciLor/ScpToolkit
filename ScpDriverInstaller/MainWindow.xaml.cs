using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Repository.Hierarchy;
using Mantin.Controls.Wpf.Notification;
using Ookii.Dialogs.Wpf;
using ScpDriverInstaller.Properties;
using ScpDriverInstaller.Utilities;
using ScpDriverInstaller.View_Models;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Navigation;
using DeviceEnumerator;
using ScpDriverInstaller.ScpComponents;
using Utilites;

namespace ScpDriverInstaller
{
    /// <summary>
    ///     Where the wizard does its black magic
    /// </summary>
    public partial class MainWindow : Window, IAppender
    {
        public MainWindow()
        {
            InitializeComponent();

            AppDomain.CurrentDomain.UnhandledException +=
                (sender, args) =>
                {
                    Log.FatalFormat("An unexpected exception occurred: {0}", args.ExceptionObject);
                };
        }

        public void DoAppend(LoggingEvent loggingEvent)
        {
            if (!IsInitialized)
                return;

            var level = loggingEvent.Level;

            if (level == Level.Info)
            {
                Dispatcher.Invoke(
                    () => ShowPopup("Information", loggingEvent.RenderedMessage, NotificationType.Information));
            }

            if (level == Level.Warn)
            {
                Dispatcher.Invoke(
                    () => ShowPopup("Warning", loggingEvent.RenderedMessage, NotificationType.Warning));
            }

            if (level == Level.Error)
            {
                Dispatcher.Invoke(
                    () => ShowPopup("Error", loggingEvent.RenderedMessage, NotificationType.Error));
            }
        }

        #region Private methods

        private void ShowPopup(string title, string message, NotificationType type)
        {
            var popup = new ToastPopUp(title, message, type)
            {
                Background = Background,
                FontFamily = FontFamily
            };

            popup.Show();
        }

        #endregion Private methods


        #region Wizard events

        private void Wizard_OnHelp(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/nefarius/ScpToolkit/wiki");
        }

        private void Hyperlink_OnRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(e.Uri.ToString());
        }

        #endregion Wizard events

        #region Private static fields

        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private InstallationOptionsViewModel ViewModel => (InstallationOptionsViewModel) DataContext;

        #endregion Private static fields

        #region Button events

        private bool _isRebootRequired = false;
        private DeviceEnumeratorWinApi _enumerator;
        private void InstallSomeService(IInstallable component, string title,
            string onSuccesTitle, string onSuccesContent)
        {
            MainBusyIndicator.IsBusy = true;
            try
            {
                MainBusyIndicator.SetContentThreadSafe(title);
                component.Install();
                _isRebootRequired |= component.IsRebootRequired;

                ExtendedMessageBox.Show(this,
                    Properties.Resources.SetupSuccessTitle,
                    onSuccesTitle,
                    onSuccesContent,
                    string.Empty,
                    string.Empty,
                    TaskDialogIcon.Information);

            }
            catch (Win32Exception e)
            {
                ExtendedMessageBox.Show(this,
                    Properties.Resources.SetupFailedTitle,
                    Properties.Resources.SetupFailedInstructions,
                    Properties.Resources.SetupFailedContent,
                    string.Format(Properties.Resources.SetupFailedVerbose, e, e.NativeErrorCode),
                    Properties.Resources.SetupFailedFooter,
                    TaskDialogIcon.Error);
            }
            catch (Exception e)
            {
                ExtendedMessageBox.Show(this,
                    Properties.Resources.SetupFailedTitle,
                    Properties.Resources.SetupFailedInstructions,
                    Properties.Resources.SetupFailedContent,
                    string.Format(Properties.Resources.SetupFailedVerbose, e, e.Message),
                    Properties.Resources.SetupFailedFooter,
                    TaskDialogIcon.Error);
            }
            finally
            {
                MainBusyIndicator.IsBusy = false;
            }
        }
        private void InstallDsOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            //TODO: async
            InstallSomeService(UsbInstallers.GetDs3Installer(),
                Properties.Resources.DualShockSetupInstalling3,
                Properties.Resources.DualShockSetupSuccessInstruction,
                Properties.Resources.SetupSuccessContent
            );
            InstallSomeService(UsbInstallers.GetDs4Installer(),
                Properties.Resources.DualShockSetupInstalling4,
                Properties.Resources.DualShockSetupSuccessInstruction,
                Properties.Resources.SetupSuccessContent
            );
            InstallSomeService(UsbInstallers.GetBtInstaller(),
                Properties.Resources.BluetoothSetupInstalling,
                Properties.Resources.BluetoothSetupSuccessInstruction,
                Properties.Resources.SetupSuccessContent
            );
            InstallSomeService(new VBusDriverInstaller(Log),
                Properties.Resources.VirtualBusSetupInstalling,
                Properties.Resources.VirtualBusSetupSuccessInstruction,
                Properties.Resources.SetupSuccessContent
            );

            InstallSomeService(new WindowsServiceInstaller(Log),
                Properties.Resources.ServiceSetupInstalling,
                Properties.Resources.ServiceSetupSuccessInstruction,
                Properties.Resources.ServiceSetupSuccessContent
            );
        }
        #endregion Button events

        #region Window events

        private void Window_Initialized(object sender, EventArgs e)
        {
            Log.InfoFormat("SCP Driver Installer {0} [Built: {1}]", Assembly.GetExecutingAssembly().GetName().Version,
                AssemblyHelper.LinkerTimestamp);

            Log.InfoFormat("{0} detected", OsInfoHelper.OsInfo);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // add popup-appender to all loggers
            foreach (var currentLogger in LogManager.GetCurrentLoggers())
            {
                ((Logger)currentLogger.Logger).AddAppender(this);
            }

            // stop service if exists so no device is occupied
            if (ServiceControl.StopService(Log))
            {
                Log.InfoFormat("{0} stopped", Settings.Default.ScpServiceName);
            }

#if NOPE
    // link download progress to progress bar
            RedistPackageInstaller.Instance.ProgressChanged +=
                (o, args) => { Dispatcher.Invoke(() => MainProgressBar.Value = args.CurrentProgressPercentage); };

            // link NotifyAppender to TextBlock
            foreach (
                var appender in
                    LogManager.GetCurrentLoggers()
                        .SelectMany(log => log.Logger.Repository.GetAppenders().OfType<NotifyAppender>()))
            {
                LogTextBlock.DataContext = appender;
            }
#endif
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            if (_isRebootRequired)
            {
                MessageBox.Show(this,
                    Properties.Resources.RebootRequiredContent,
                    Properties.Resources.RebootRequiredTitle,
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }

            // remove popup-appender from all loggers
            foreach (var currentLogger in LogManager.GetCurrentLoggers())
            {
                ((Logger)currentLogger.Logger).RemoveAppender(this);
            }

            if (ServiceControl.StartService(Log))
            {
                Log.InfoFormat("{0} started", Settings.Default.ScpServiceName);
            }
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            // refresh all lists
            //TODO:OnUsbDeviceAddedOrRemoved();

            // hook into WndProc
            var source = PresentationSource.FromVisual(this) as HwndSource;
            //TODO:source?.AddHook(WndProc);
            //TODO:            _enumerator = new DeviceEnumeratorWinApi(source);
        }
        #endregion Window events
    }
}