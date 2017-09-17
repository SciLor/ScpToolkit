using System;
using HidReport.Contract.Enums;
using NativeLayer.Contract;
using NativeLayer.UsbDevices;

namespace NativeLayer.Usb.Gamepads
{
    //if (classGuid == DeviceClassGuids.UsbTwinUsbJoystick)
    //{
    //    Log.Info("GameStop PC Advanced Controller detected");
    //    return new UsbTwinUsbJoystick();
    //}
    internal sealed class UsbTwinUsbJoystick : UsbGenericGamepad
    {
        protected override void ParseHidReport(byte[] report)
        {
            if (report[0] != 0x01) return;

            var inputReport = NewHidReport();

            #region HID Report translation

            // no battery state since the Gamepad is Usb-powered
            Battery = DsBattery.None;

            // packet counter
            inputReport.PacketCounter = ++PacketCounter;

            inputReport.Set(ButtonsEnum.Circle, BitUtils.IsBitSet(report[5], 5));
            inputReport.Set(ButtonsEnum.Cross, BitUtils.IsBitSet(report[5], 6));
            inputReport.Set(ButtonsEnum.Triangle, BitUtils.IsBitSet(report[5], 4));
            inputReport.Set(ButtonsEnum.Triangle, BitUtils.IsBitSet(report[5], 7));

            // TODO: implement!

            #endregion

            OnHidReportReceived(inputReport);
        }

        public override Guid DeviceClassGuid { get; } = DeviceClassGuids.UsbTwinUsbJoystick;
    }
}
