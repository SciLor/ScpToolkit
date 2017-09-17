﻿using System;
using System.Reflection;
using System.Windows;
using log4net;

namespace ScpProfiler
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnStartup(StartupEventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                Log.FatalFormat("An unexpected error occurred on application startup: {0}", args.ExceptionObject);

                MessageBox.Show("A fatal error occurred. Consult the logs for details.",
                    "Oh sh...", MessageBoxButton.OK, MessageBoxImage.Error);
            };

            base.OnStartup(e);
        }
    }
}
