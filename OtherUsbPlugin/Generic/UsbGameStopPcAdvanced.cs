using System;
using HidReport.Contract.Enums;
using NativeLayer.Contract;
using NativeLayer.UsbDevices;

namespace NativeLayer.Usb.Gamepads
{
    /// <summary>
    ///     GameStop PC Advanced Controller
    /// </summary>
    ///                         //if (classGuid == DeviceClassGuids.UsbGameStopPc)
    //{
    //    Log.Info("GameStop PC Advanced Controller detected");
    //    return new UsbGameStopPcAdvanced();
    //}
    internal sealed class UsbGameStopPcAdvanced : UsbGenericGamepad
    {
        protected override void ParseHidReport(byte[] report)
        {
            if (report[8] != 0xC0 && report[8] != 0x40) return;

            PacketCounter++;

            var inputReport = NewHidReport();

            #region HID Report translation

            // no battery state since the Gamepad is Usb-powered
            Battery = DsBattery.None;

            // packet counter
            inputReport.PacketCounter = PacketCounter;

            // buttons equaly reported in both modes
            inputReport.Set(ButtonsEnum.Circle, BitUtils.IsBitSet(report[6], 5));
            inputReport.Set(ButtonsEnum.Cross, BitUtils.IsBitSet(report[6], 6));
            inputReport.Set(ButtonsEnum.Triangle, BitUtils.IsBitSet(report[6], 4));
            inputReport.Set(ButtonsEnum.Square, BitUtils.IsBitSet(report[6], 7));

            inputReport.Set(ButtonsEnum.Select, BitUtils.IsBitSet(report[7], 4));
            inputReport.Set(ButtonsEnum.Start, BitUtils.IsBitSet(report[7], 5));

            inputReport.Set(ButtonsEnum.L1, BitUtils.IsBitSet(report[7], 0));
            inputReport.Set(ButtonsEnum.R1, BitUtils.IsBitSet(report[7], 1));
            inputReport.Set(ButtonsEnum.L2, BitUtils.IsBitSet(report[7], 2));
            inputReport.Set(ButtonsEnum.R2, BitUtils.IsBitSet(report[7], 3));

            inputReport.Set(ButtonsEnum.L3, BitUtils.IsBitSet(report[7], 6));
            inputReport.Set(ButtonsEnum.R3, BitUtils.IsBitSet(report[7], 7));

            // detect mode it's running in
            switch (report[8])
            {
                case 0xC0: // mode 1
                {
                    inputReport.Set(ButtonsEnum.Up, (report[2] == 0x00));
                    inputReport.Set(ButtonsEnum.Right, (report[1] == 0xFF));
                    inputReport.Set(ButtonsEnum.Down, (report[2] == 0xFF));
                    inputReport.Set(ButtonsEnum.Left, (report[1] == 0x00));

                    // mode 1 doesn't report the thumb sticks
                    inputReport.Set(AxesEnum.Lx, 0x80);
                    inputReport.Set(AxesEnum.Ly, 0x80);
                    inputReport.Set(AxesEnum.Rx, 0x80);
                    inputReport.Set(AxesEnum.Ry, 0x80);
                }
                    break;
                case 0x40: // mode 2
                {
                    var dPad = (byte) (report[6] & ~0xF0);

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

                    inputReport.Set(AxesEnum.Lx, report[1]);
                    inputReport.Set(AxesEnum.Ly, report[2]);

                    inputReport.Set(AxesEnum.Rx, report[4]);
                    inputReport.Set(AxesEnum.Ry, report[5]);
                }
                    break;
            }

            #endregion

            OnHidReportReceived(inputReport);
        }

        public override Guid DeviceClassGuid { get; } = DeviceClassGuids.UsbGameStopPc;
    }
}