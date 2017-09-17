using System;
using HidReport.Contract.Enums;
using NativeLayer.Contract;
using NativeLayer.UsbDevices;

namespace NativeLayer.Usb.Gamepads
{
    /// <summary>
    ///     ShanWan Wireless Gamepad
    /// </summary>
    /// 
    // DeviceClassGuids.UsbShanWan
    //ShanWan Wireless Gamepad
    internal sealed class UsbShanWanWirelessGamepad : UsbGenericGamepad
    {
        protected override void ParseHidReport(byte[] report)
        {
            if (report[7] != 0x00) return;

            if (PacketCounter++ + 1 < PacketCounter)
            {
                Log.WarnFormat("Packet counter rolled over ({0}), resetting to 0", PacketCounter);
                PacketCounter = 0;
            }

            var inputReport = NewHidReport();

            #region HID Report translation

            // no battery state since the Gamepad is Usb-powered
            Battery = DsBattery.None;

            // packet counter
            inputReport.PacketCounter = PacketCounter;

            inputReport.Set(ButtonsEnum.Circle, BitUtils.IsBitSet(report[5], 5));
            inputReport.Set(ButtonsEnum.Cross, BitUtils.IsBitSet(report[5], 6));

            inputReport.Set(ButtonsEnum.Select, BitUtils.IsBitSet(report[6], 4));
            inputReport.Set(ButtonsEnum.Start, BitUtils.IsBitSet(report[6], 5));

            var dPad = (byte)(report[5] & ~0xF0);

            switch (dPad)
            {
                case 0:
                    inputReport.Set(ButtonsEnum.Up);
                    break;
                case 1:
                    inputReport.Set(ButtonsEnum.Up);
                    inputReport.Set(ButtonsEnum.Right);
                    break;
                case 2:
                    inputReport.Set(ButtonsEnum.Right);
                    break;
                case 3:
                    inputReport.Set(ButtonsEnum.Right);
                    inputReport.Set(ButtonsEnum.Down);
                    break;
                case 4:
                    inputReport.Set(ButtonsEnum.Down);
                    break;
                case 5:
                    inputReport.Set(ButtonsEnum.Down);
                    inputReport.Set(ButtonsEnum.Left);
                    break;
                case 6:
                    inputReport.Set(ButtonsEnum.Left);
                    break;
                case 7:
                    inputReport.Set(ButtonsEnum.Left);
                    inputReport.Set(ButtonsEnum.Up);
                    break;
            }

            inputReport.Set(ButtonsEnum.Triangle, BitUtils.IsBitSet(report[5], 4));
            inputReport.Set(ButtonsEnum.Square, BitUtils.IsBitSet(report[5], 7));

            inputReport.Set(ButtonsEnum.L1, BitUtils.IsBitSet(report[6], 0));
            inputReport.Set(ButtonsEnum.R1, BitUtils.IsBitSet(report[6], 1));

            inputReport.Set(ButtonsEnum.L2, BitUtils.IsBitSet(report[6], 2));
            inputReport.Set(ButtonsEnum.R2, BitUtils.IsBitSet(report[6], 3));

            inputReport.Set(ButtonsEnum.L3, BitUtils.IsBitSet(report[6], 6));
            inputReport.Set(ButtonsEnum.R3, BitUtils.IsBitSet(report[6], 7));

            inputReport.Set(AxesEnum.Lx, report[3]);
            inputReport.Set(AxesEnum.Ly, report[4]);

            inputReport.Set(AxesEnum.Rx, report[1]);
            inputReport.Set(AxesEnum.Ry, report[2]);
            
            #endregion

            OnHidReportReceived(inputReport);
        }

        public override Guid DeviceClassGuid { get; } = DeviceClassGuids.UsbShanWan;
    }
}
