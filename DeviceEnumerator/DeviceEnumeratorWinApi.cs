using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NativeLayer.Contract;
using NativeLayer.Driver;
using NativeLayer.Usb.PnP;
using NativeLayer.Utilities;

namespace DeviceEnumerator
{
    public class DeviceEnumeratorWinApi
    {
        private readonly UsbNotifier _genericBluetoothHost =
            new UsbNotifier(DeviceClassGuids.GUID_BTHPORT_DEVICE_INTERFACE);

        Regex regex = new Regex("VID_([0-9A-Z]{4})&PID_([0-9A-Z]{4})", RegexOptions.IgnoreCase);
        private IEnumerable<string> supportedBluetoothDevices;
        public DeviceEnumeratorWinApi(IntPtr hwnd)
        {
            _hWnd = hwnd;
            supportedBluetoothDevices = IniConfig.Instance.BthDongleDriver.HardwareIds;
        }

        private IEnumerable<WdiDeviceInfo> _hisUsb => usbDevices.Where(
            d => d.VendorId == _hidUsbDs3.VendorId
                 && (d.ProductId == _hidUsbDs3.ProductId || d.ProductId == _hidUsbDs4.ProductId)
                 && !string.IsNullOrEmpty(d.CurrentDriver)
                 && d.CurrentDriver.Equals("HidUsb"));

        private IEnumerable<WdiDeviceInfo> _winUsb => usbDevices.Where(
            d => d.VendorId == _hidUsbDs3.VendorId
                 && (d.ProductId == _hidUsbDs3.ProductId || d.ProductId == _hidUsbDs4.ProductId)
                 && !string.IsNullOrEmpty(d.CurrentDriver)
                 && d.CurrentDriver.Equals("WinUSB"));

        private IEnumerable<WdiDeviceInfo> uninitialized = usbDevices.Where(
        d =>
            !string.IsNullOrEmpty(d.CurrentDriver) &&
            d.CurrentDriver.Equals("BTHUSB") &&
            supportedBluetoothDevices.Any(s => s.Contains(regex.Match(d.HardwareId).Value)));

        // refresh devices filtering on supported Bluetooth hardware IDs and WinUSB driver (initialized)
        private IEnumerable<WdiDeviceInfo> initialized =
    usbDevices.Where(
        d =>
            !string.IsNullOrEmpty(d.CurrentDriver) &&
            d.CurrentDriver.Equals("WinUSB") &&
            supportedBluetoothDevices.Any(s => s.Contains(regex.Match(d.HardwareId).Value)));

        private List<WdiDeviceInfo> usbDevices;
        private void OnUsbDeviceAddedOrRemoved()
        {
            usbDevices = WdiWrapper.Instance.UsbDeviceList.ToList();
        }

    }
}
