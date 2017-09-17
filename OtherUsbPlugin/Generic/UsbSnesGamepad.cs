using System;
using HidReport.Contract.Enums;
using NativeLayer.Contract;
using NativeLayer.UsbDevices;

namespace NativeLayer.Usb.Gamepads
{
    /// <summary>
    ///     DragonRise Inc. Usb Gamepad SNES
    /// </summary>
    ///                         //if (classGuid == DeviceClassGuids.UsbDragonRise)
    //{
    //    Log.Info("DragonRise Inc. Usb Gamepad SNES detected");
    //    return new UsbSnesGamepad();
    //}
    //if (classGuid == DeviceClassGuids.UsbSnesGamedap)
    //{
    //    Log.Info("GameStop PC Advanced Controller detected");
    //    return new UsbSnesGamepad();
    //}
    internal sealed class UsbSnesGamepad : UsbGenericGamepad
    {

        protected override void ParseHidReport(byte[] report)
        {
            if (report[1] != 0x01) return;

            PacketCounter++;

            var inputReport = NewHidReport();

            #region HID Report translation

            // no battery state since the Gamepad is Usb-powered
            Battery = DsBattery.None;

            // packet counter
            inputReport.PacketCounter = PacketCounter;

            inputReport.Set(ButtonsEnum.Triangle, BitUtils.IsBitSet(report[6], 4)); // Triangle (button)
            inputReport.Set(ButtonsEnum.Circle, BitUtils.IsBitSet(report[6], 5)); // Circle (button)
            inputReport.Set(ButtonsEnum.Cross, BitUtils.IsBitSet(report[6], 6)); // Cross (button)
            inputReport.Set(ButtonsEnum.Square, BitUtils.IsBitSet(report[6], 7)); // Square (button)
            inputReport.Set(ButtonsEnum.L1, BitUtils.IsBitSet(report[7], 0)); // L1 (button)
            inputReport.Set(ButtonsEnum.R1, BitUtils.IsBitSet(report[7], 2)); // R1 (button)
            inputReport.Set(ButtonsEnum.Select, BitUtils.IsBitSet(report[7], 4)); // Select
            inputReport.Set(ButtonsEnum.Start, BitUtils.IsBitSet(report[7], 5)); // Start
            inputReport.Set(ButtonsEnum.Right, (report[4] == 0xFF)); // D-Pad right
            inputReport.Set(ButtonsEnum.Left, (report[4] == 0x00)); // D-Pad left
            inputReport.Set(ButtonsEnum.Up, (report[5] == 0x00)); // D-Pad up
            inputReport.Set(ButtonsEnum.Down, (report[5] == 0xFF)); // D-Pad down

            // This device has no thumb sticks, center axes
            inputReport.Set(AxesEnum.Lx, 0x80);
            inputReport.Set(AxesEnum.Ly, 0x80);
            inputReport.Set(AxesEnum.Rx, 0x80);
            inputReport.Set(AxesEnum.Ry, 0x80);

            #endregion

            OnHidReportReceived(inputReport);
        }

        public override Guid DeviceClassGuid { get; } = DeviceClassGuids.UsbSnesGamedap;
    }
}
