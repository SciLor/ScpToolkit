using System;
using HidReport.Contract.Enums;
using HidReport.Core;
using NativeLayer.Contract;
using NativeLayer.PacketParsers;
using NativeLayer.Rawffsets.Ds3;
using NativeLayer.UsbDevices;

namespace NativeLayer.Usb.Ds3.Replica
{
    /// <summary>
    ///     Represents a <see href="http://www.quadstick.com/">Quad Stick</see> controller.
    /// </summary>
    // it currently behaves identical to the Afterglow controller, no additional changes needed

    //DeviceClassGuids.UsbAfterglow
    //Afterglow AP.2 Wireless Controller for PS3
    //Quad Stick
    //TODO: apply workaround
    internal sealed class UsbDs3Afterglow : UsbDs3Base
    {
        public override Guid DeviceClassGuid { get; } = DeviceClassGuids.UsbAfterglow;

        protected override void ParseHidReport(byte[] report)
        {
            if (report[26] != 0x02) return;

            PacketCounter++;

            ScpHidReport inputReport = HidReport;
            #region HID Report translation

            // battery
            inputReport.BatteryStatus = (DsBattery) report[30];
            inputReport.PacketCounter = PacketCounter;
            inputReport.ReportId = report[Ds3Offsets.ReportId];

            // packet counter
            inputReport.PacketCounter = PacketCounter;

            inputReport.Set(ButtonsEnum.Square, BitUtils.IsBitSet(report[0], 0));
            inputReport.Set(ButtonsEnum.Cross, BitUtils.IsBitSet(report[0], 1));
            inputReport.Set(ButtonsEnum.Circle, BitUtils.IsBitSet(report[0], 2));
            inputReport.Set(ButtonsEnum.Triangle, BitUtils.IsBitSet(report[0], 3));
            inputReport.Set(ButtonsEnum.L1, BitUtils.IsBitSet(report[0], 4));
            inputReport.Set(ButtonsEnum.R1, BitUtils.IsBitSet(report[0], 5));
            inputReport.Set(ButtonsEnum.L2, BitUtils.IsBitSet(report[0], 6));
            inputReport.Set(ButtonsEnum.R2, BitUtils.IsBitSet(report[0], 7));
            inputReport.Set(ButtonsEnum.Select, BitUtils.IsBitSet(report[1], 0));
            inputReport.Set(ButtonsEnum.Start, BitUtils.IsBitSet(report[1], 1));
            inputReport.Set(ButtonsEnum.L3, BitUtils.IsBitSet(report[1], 2));
            inputReport.Set(ButtonsEnum.R3, BitUtils.IsBitSet(report[1], 3));
            inputReport.Set(ButtonsEnum.Ps, BitUtils.IsBitSet(report[1], 4));

            inputReport.Set(AxesEnum.Lx, report[3]);
            inputReport.Set(AxesEnum.Ly, report[4]);
            inputReport.Set(AxesEnum.Rx, report[5]);
            inputReport.Set(AxesEnum.Ry, report[6]);

            inputReport.Set(AxesEnum.Triangle, report[11]);
            inputReport.Set(AxesEnum.Circle, report[12]);
            inputReport.Set(AxesEnum.Cross, report[13]);
            inputReport.Set(AxesEnum.Square, report[14]);
            inputReport.Set(AxesEnum.L1, report[15]);
            inputReport.Set(AxesEnum.R1, report[16]);
            inputReport.Set(AxesEnum.L2, report[17]);
            inputReport.Set(AxesEnum.R2, report[18]);

            // D-Pad
            if (report[2] != 0x0F)
            {
                var direction = report[2];
                Parsers.DirToInputReport(direction, inputReport);
            }
            #endregion

            OnHidReportReceived(inputReport);
        }
    }
}
