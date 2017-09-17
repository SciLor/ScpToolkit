using System;
using System.ComponentModel;
using System.ServiceProcess;
using log4net;
using ScpDriverInstaller.Properties;

namespace ScpDriverInstaller.Utilities
{
    internal static class ServiceControl
    {

        private static bool StartService(string service, ILog log)
        {
            try
            {
                var sc = new ServiceController(service);

                if (sc.Status == ServiceControllerStatus.Stopped)
                {
                    sc.Start();
                    sc.WaitForStatus(ServiceControllerStatus.Running);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.ErrorFormat("Couldn't start service: {0}", ex);
            }

            return false;
        }

        public static bool StartService(ILog log)
        {
            return StartService(Settings.Default.ScpServiceName, log);
        }

        public static bool StopService(ILog log)
        {
            return StopService(Settings.Default.ScpServiceName, log);
        }

        public static bool StopService(string service, ILog log)
        {
            try
            {
                var sc = new ServiceController(service);

                if (sc.Status == ServiceControllerStatus.Running)
                {
                    sc.Stop();
                    sc.WaitForStatus(ServiceControllerStatus.Stopped);
                    return true;
                }
            }
            catch (InvalidOperationException iopex)
            {
                if (!(iopex.InnerException is Win32Exception))
                {
                    log.ErrorFormat("Win32-Exception occured: {0}", iopex);
                    return false;
                }

                switch (((Win32Exception)iopex.InnerException).NativeErrorCode)
                {
                    case 1060: // ERROR_SERVICE_DOES_NOT_EXIST
                        log.Warn("Service doesn't exist, maybe it was uninstalled before");
                        break;

                    default:
                        log.ErrorFormat("Win32-Error: {0}", (Win32Exception)iopex.InnerException);
                        break;
                }
            }
            catch (Exception ex)
            {
                log.ErrorFormat("Couldn't stop service: {0}", ex);
            }

            return false;
        }
    }
}
