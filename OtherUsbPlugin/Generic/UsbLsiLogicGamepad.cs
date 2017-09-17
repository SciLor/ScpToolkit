using System;
using HidReport.Contract.Enums;
using NativeLayer.Contract;
using NativeLayer.UsbDevices;

namespace NativeLayer.Usb.Gamepads
{
    /// <summary>
    ///     LSI Logic Gamepad
    /// </summary>
    //DeviceClassGuids.UsbLsiLogic
    //LSI Logic Gamepad
    internal sealed class UsbLsiLogicGamepad : UsbGenericGamepad
    {

        protected override void ParseHidReport(byte[] report)
        {
            if (report[2] != 0x00) return;

            var inputReport = NewHidReport();

            #region HID Report translation

            // no battery state since the Gamepad is Usb-powered
            Battery = DsBattery.None;

            // packet counter
            inputReport.PacketCounter = ++PacketCounter;

            // control buttons
            inputReport.Set(ButtonsEnum.Select, BitUtils.IsBitSet(report[6], 4));
            inputReport.Set(ButtonsEnum.Start, BitUtils.IsBitSet(report[6], 5));

            // Left shoulder
            inputReport.Set(ButtonsEnum.L1, BitUtils.IsBitSet(report[6], 0));

            // Right shoulder
            inputReport.Set(ButtonsEnum.R1, BitUtils.IsBitSet(report[6], 1));

            // Left trigger
            inputReport.Set(ButtonsEnum.L2, BitUtils.IsBitSet(report[6], 2));

            // Right trigger
            inputReport.Set(ButtonsEnum.R2, BitUtils.IsBitSet(report[6], 3));

            // Triangle
            inputReport.Set(ButtonsEnum.Triangle, BitUtils.IsBitSet(report[5], 4));

            // Circle
            inputReport.Set(ButtonsEnum.Circle, BitUtils.IsBitSet(report[5], 5));

            // Cross
            inputReport.Set(ButtonsEnum.Cross, BitUtils.IsBitSet(report[5], 6));

            // Square
            inputReport.Set(ButtonsEnum.Square, BitUtils.IsBitSet(report[5], 7));

            // Left thumb
            inputReport.Set(ButtonsEnum.L3, BitUtils.IsBitSet(report[6], 6));
            // Right thumb
            inputReport.Set(ButtonsEnum.R3, BitUtils.IsBitSet(report[6], 7));

            var dPad = (byte)(report[5] & ~0xF0);

            // D-Pad
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

            // Left thumb stick
            inputReport.Set(AxesEnum.Lx, report[0]);
            inputReport.Set(AxesEnum.Ly, report[1]);
            
            // Right thumb stick
            inputReport.Set(AxesEnum.Rx, report[3]);
            inputReport.Set(AxesEnum.Ry, report[4]);
            
            #endregion

            OnHidReportReceived(inputReport);
        }

        public override Guid DeviceClassGuid { get; } = DeviceClassGuids.UsbLsiLogic;
    }
}
