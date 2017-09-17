using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Config;
using log4net;
using NativeLayer.Contract;
using NativeLayer.Database;
using NativeLayer.Driver;

namespace ScpDriverInstaller.ScpComponents
{
=

    internal static class WdiDriverInstallerFactory
    {
        public static WdiDriverInstaller Ds3()
        {
            WdiDeviceInfo deviceInfo = null;
            return new WdiDriverInstaller(
                "Ds3Controller",
                deviceInfo,
                DeviceClassGuids.UsbDs3);
        }

        public static WdiDriverInstaller Ds4()
        {
            WdiDeviceInfo deviceInfo = null;
            return new WdiDriverInstaller(
                "Ds4Controller",
                deviceInfo,
                DeviceClassGuids.UsbDs4);
        }

        public static WdiDriverInstaller Bluetooth()
        {
            WdiDeviceInfo deviceInfo = null;
            return new WdiDriverInstaller(
                "BluetoothHost", //BthDongle_
                deviceInfo,
                DeviceClassGuids.BthDongle);
        }
    }

    internal class WdiDriverInstaller: IInstallable
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly string _infFilePrefix;

        private readonly WdiDeviceInfo _usbDevice;
        private readonly Guid _deviceClassGuid;
        private IntPtr hWnd;

        public WdiDriverInstaller(string infFilePrefix, WdiDeviceInfo usbDevice, Guid deviceClassGuid)
        {
            _infFilePrefix = infFilePrefix;
            _usbDevice = usbDevice;
            _deviceClassGuid = deviceClassGuid;
        }

        public void Install()
        {
            // $"{_infFilePrefix}_{_usbDevice.VendorId:X4}_{_usbDevice.ProductId:X4}.inf";
            //using (var db = new ScpDb())
            //{
            //    db.Engine.PutDbEntity(ScpDb.TableDevices, _usbDevice.DeviceId, _usbDevice);
            //}
        }

        public void Clean()
        {

        }

        public bool IsRebootRequired { get; private set; }
    }
}
