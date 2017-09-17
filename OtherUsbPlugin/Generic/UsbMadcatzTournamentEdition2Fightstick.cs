using System;
using HidReport.Contract.Enums;
using NativeLayer.Contract;
using NativeLayer.UsbDevices;

namespace NativeLayer.Usb.Gamepads
{
    //if (classGuid == DeviceClassGuids.UsbMadcatzTournamentEdition2Fightstick)
    //{
    //    Log.Info("GameStop PC Advanced Controller detected");
    //    return new UsbMadcatzTournamentEdition2Fightstick();
    //}
    internal sealed class UsbMadcatzTournamentEdition2Fightstick : UsbGenericGamepad
    {
        protected override void ParseHidReport(byte[] report)
        {
            PacketCounter++;

            var inputReport = NewHidReport();

            #region HID Report translation

            // no battery state since the Gamepad is Usb-powered
            Battery = DsBattery.None;

            // packet counter
            inputReport.PacketCounter = PacketCounter;

            // circle
            inputReport.Set(ButtonsEnum.Circle, BitUtils.IsBitSet(report[1], 2));
            inputReport.Set(AxesEnum.Circle, report[13]);

            // cross
            inputReport.Set(ButtonsEnum.Cross, BitUtils.IsBitSet(report[1], 1));
            inputReport.Set(AxesEnum.Cross, report[14]);

            // triangle
            inputReport.Set(ButtonsEnum.Triangle, BitUtils.IsBitSet(report[1], 3));
            inputReport.Set(AxesEnum.Triangle, report[12]);

            // square
            inputReport.Set(ButtonsEnum.Square, BitUtils.IsBitSet(report[1], 0));
            inputReport.Set(AxesEnum.Square, report[15]);

            // select
            inputReport.Set(ButtonsEnum.Select, BitUtils.IsBitSet(report[2], 0));

            // start
            inputReport.Set(ButtonsEnum.Start, BitUtils.IsBitSet(report[2], 1));



            #endregion

            OnHidReportReceived(inputReport);
        }

        public override Guid DeviceClassGuid { get; } = DeviceClassGuids.UsbMadcatzTournamentEdition2Fightstick;
    }
}
