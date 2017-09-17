using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using Config;
using DBreeze;
using log4net;
using NativeLayer.Contract;
using NativeLayer.Driver;
using ScpDriverInstaller.Properties;

namespace ScpDriverInstaller.ScpComponents
{
    internal class VBusDriverInstaller : IInstallable
    {
        public enum State
        {
            None,
            VirtualBusSetupAddingDriverStore,
            VirtualBusSetupCreating,
            VirtualBusSetupInstalling
        }
        private readonly ILog _log;
        public VBusDriverInstaller(ILog log)
        {
            _log = log;
        }

        public void Install()
        {

            string devPath = string.Empty, instanceId = string.Empty;

            var busInfPath = Path.Combine(
                GlobalConfiguration.AppDirectory,
                "ScpVBus",
                Environment.Is64BitOperatingSystem ? "amd64" : "x86",
                "ScpVBus.inf");
            _log.DebugFormat("ScpVBus.inf path: {0}", busInfPath);

            // check for existence of Scp VBus
            if (!Devcon.Find(DBreezeResources.Settings.Default.VirtualBusClassGuid, ref devPath, ref instanceId))
            {
                CurrentState = State.VirtualBusSetupAddingDriverStore;
                bool rebootRequired = false;
                // if not detected, install Inf-file in Windows Driver Store
                if (!Devcon.Install(busInfPath, ref rebootRequired))
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error(),
                        "Virtual Bus Driver pre-installation failed");
                }
                IsRebootRequired |= rebootRequired;
                _log.Info("Virtual Bus Driver pre-installed in Windows Driver Store successfully");

                CurrentState = State.VirtualBusSetupCreating;

                // create pseudo-device so the bus driver can attach to it later
                if (!Devcon.Create("System", DeviceClassGuids.VirtualBusPseudoDeviceClass,
                    "root\\ScpVBus\0\0"))
                    throw new Exception("Virtual Bus Device creation failed");
                _log.Info("Virtual Bus Created");
            }

            CurrentState = State.VirtualBusSetupInstalling;

            bool isRebootRequired = false;
            // install Virtual Bus driver
            var result = Difx.Instance.Install(busInfPath,
                DifxFlags.DRIVER_PACKAGE_ONLY_IF_DEVICE_PRESENT | DifxFlags.DRIVER_PACKAGE_FORCE,
                out isRebootRequired);
            IsRebootRequired |= isRebootRequired;

            if (result != 0)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error(), "Virtual Bus driver installation failed");
            }
        }

        public bool IsRebootRequired { get; private set; }

        //TODO: use this in UI;
        public State CurrentState { get; private set; } = State.None;

        public void Unistall()
        {

            string devPath = string.Empty;
            string instanceId = string.Empty;

            if (Devcon.Find(DeviceClassGuids.VirtualBusClass, ref devPath, ref instanceId))
            {
                if (Devcon.Remove(DeviceClassGuids.VirtualBusClass, devPath, instanceId))
                {
                    bool rebootRequired;
                    Difx.Instance.Uninstall(Path.Combine(@".\System\", @"ScpVBus.inf"),
                        DifxFlags.DRIVER_PACKAGE_DELETE_FILES,
                        out rebootRequired);
                    IsRebootRequired |= rebootRequired;

                }
            
            }
        }
    }

}
