using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.IO;
using Config;
using log4net;
using ScpDriverInstaller.Properties;
using ScpDriverInstaller.Utilities;

namespace ScpDriverInstaller.ScpComponents
{
    internal class WindowsServiceInstaller: IInstallable
    {
        private readonly ILog _log;
        public WindowsServiceInstaller(ILog log)
        {
            _log = log;
        }
        public void Install()
        {
            try
            {

                IDictionary state = new Hashtable();
                var service =
                    new AssemblyInstaller(Path.Combine(GlobalConfiguration.AppDirectory, "ScpService.exe"), null);

                state.Clear();
                service.UseNewContext = true;

                service.Install(state);
                service.Commit(state);
                if (ServiceControl.StartService(_log))
                {
                    _log.InfoFormat("{0} started", Settings.Default.ScpServiceName);
                }
                else
                {
                    IsRebootRequired = true;
                }
            }
            catch (Win32Exception w32Ex)
            {
                switch (w32Ex.NativeErrorCode)
                {
                    case 1073: // ERROR_SERVICE_EXISTS
                        _log.Info("Service already exists");
                        break;
                    default:
                        throw new Exception($"Win32-Error during installation: {w32Ex}");
                }
            }
            catch (InvalidOperationException iopex)
            {
                throw new Exception($"Error during installation: {iopex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error during installation: {ex}");
            }
        }

        public void Uninstall()
        {
            var Log = _log;
            Log.InfoFormat("Stopping \"SCP DS3 Service\"...");
            ServiceControl.StopService("SCP DS3 Service", Log);

            Log.InfoFormat("Stopping \"SCP DSx Service\"...");
            ServiceControl.StopService("SCP DSx Service", Log);
            Log.InfoFormat("Searching for running processes...");
            foreach (var proc in Process.GetProcessesByName("Ds3Service"))
            {
                Log.InfoFormat("Killing process: {0}", proc.ProcessName);
                proc.Kill();
            }

            foreach (var proc in Process.GetProcessesByName("DsxService"))
            {
                Log.InfoFormat("Killing process: {0}", proc.ProcessName);
                proc.Kill();
            }
            Log.InfoFormat("Removing service...");
            Process.Start("sc", "delete Ds3Service").WaitForExit();

            Process.Start("sc", "delete DsxService").WaitForExit();



        }

        public bool IsRebootRequired { get; private set; }
    }
}
