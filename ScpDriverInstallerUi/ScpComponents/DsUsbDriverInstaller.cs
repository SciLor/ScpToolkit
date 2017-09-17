using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using Config;
using NativeLayer.Driver;

namespace ScpDriverInstaller.ScpComponents
{
    internal static class UsbInstallers
    {
        public static DsUsbDriverInstaller GetDs3Installer()
        {
            return new DsUsbDriverInstaller(Path.Combine(GlobalConfiguration.AppDirectory, "WinUSB", "Ds3Controller.inf"));
        }
        public static DsUsbDriverInstaller GetDs4Installer()
        {
            return new DsUsbDriverInstaller(Path.Combine(GlobalConfiguration.AppDirectory, "WinUSB", "Ds4Controller.inf"));
        }
        public static DsUsbDriverInstaller GetBtInstaller()
        {
            return new DsUsbDriverInstaller(Path.Combine(GlobalConfiguration.AppDirectory, "WinUSB", "BluetoothHost.inf"));
        }
    }

    internal class DsUsbDriverInstaller: IInstallable
    {
        // ReSharper disable once InconsistentNaming
        private const uint ERROR_NO_SUCH_DEVINST = 0xE000020B;
        private readonly string _path;
        public DsUsbDriverInstaller(string path)
        {
            _path = path;
        }

        public void Install()
        {
            Install(_path);
        }

        private void Install(string path)
        {
            bool rebootRequired;
            uint result = Difx.Instance.Install(path,
                DifxFlags.DRIVER_PACKAGE_ONLY_IF_DEVICE_PRESENT | DifxFlags.DRIVER_PACKAGE_FORCE, out rebootRequired);
            IsRebootRequired |= rebootRequired;
            if (result != 0 && result != ERROR_NO_SUCH_DEVINST)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }

        public void Uninstall()
        {

        }

        public bool IsRebootRequired { get; private set; }
    }
}
