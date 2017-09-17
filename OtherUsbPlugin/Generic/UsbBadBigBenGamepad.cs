using System;
using HidReport.Contract.Enums;
using NativeLayer.Contract;
using NativeLayer.UsbDevices;

namespace NativeLayer.Usb.Gamepads
{
    //if (classGuid == DeviceClassGuids.UsbBadBigBen)
    //{
    //    Log.Info("GameStop PC Advanced Controller detected");
    //    return new UsbBadBigBenGamepad();
    //}
    internal sealed class UsbBadBigBenGamepad : UsbGenericGamepad
    {
        protected override void ParseHidReport(byte[] report)
        {
            if (report[5] != 0x00) return;

            PacketCounter++;

            var inputReport = NewHidReport();

            #region HID Report translation

            // no battery state since the Gamepad is Usb-powered
            Battery = DsBattery.None;

            // packet counter
            inputReport.PacketCounter = PacketCounter;

            inputReport.Set(ButtonsEnum.Triangle, BitUtils.IsBitSet(report[6], 4));
            inputReport.Set(ButtonsEnum.Circle, BitUtils.IsBitSet(report[6], 5));
            inputReport.Set(ButtonsEnum.Cross, BitUtils.IsBitSet(report[6], 6));
            inputReport.Set(ButtonsEnum.Square, BitUtils.IsBitSet(report[6], 7));

            inputReport.Set(ButtonsEnum.Select, BitUtils.IsBitSet(report[7], 4));
            inputReport.Set(ButtonsEnum.Start, BitUtils.IsBitSet(report[7], 5));

            inputReport.Set(ButtonsEnum.Up, (report[4] == 0x00));
            inputReport.Set(ButtonsEnum.Right, (report[3] == 0xFF));
            inputReport.Set(ButtonsEnum.Down, (report[4] == 0xFF));
            inputReport.Set(ButtonsEnum.Left, (report[3] == 0x00));

            inputReport.Set(ButtonsEnum.L1, BitUtils.IsBitSet(report[7], 0));
            inputReport.Set(ButtonsEnum.R1, BitUtils.IsBitSet(report[7], 1));
            inputReport.Set(ButtonsEnum.L2, BitUtils.IsBitSet(report[7], 2));
            inputReport.Set(ButtonsEnum.R2, BitUtils.IsBitSet(report[7], 3));

            inputReport.Set(ButtonsEnum.L3, BitUtils.IsBitSet(report[7], 6));
            inputReport.Set(ButtonsEnum.R3, BitUtils.IsBitSet(report[7], 7));
            
            // TODO: the PS-button is dead according to the report:
            // http://forums.pcsx2.net/attachment.php?aid=57420

            #endregion

            OnHidReportReceived(inputReport);
        }

        public override Guid DeviceClassGuid { get; } = DeviceClassGuids.UsbBadBigBen;
    }
}
