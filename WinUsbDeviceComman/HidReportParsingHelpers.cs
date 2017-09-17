using System;
using System.Diagnostics;
using HidReport.Contract.Enums;
using HidReport.Core;

namespace NativeLayer.PacketParsers
{
    public static class Parsers
    {
        [Flags]
        public enum DirButtonMask
        {
            Up = 1,
            Down = 2,
            Left = 4,
            Right = 8
        }

        public static bool IsQuickDisconnectPressed(ScpHidReport inputReport)
        {

            return inputReport[ButtonsEnum.L1].IsPressed
                   && inputReport[ButtonsEnum.R1].IsPressed
                   && inputReport[ButtonsEnum.Ps].IsPressed;
        }

        public static DirButtonMask HatToPad(int direction)
        {
            //TODO: check
            switch (direction)
            {
                case 0:
                    return DirButtonMask.Up;
                case 1:
                    return (DirButtonMask.Up | DirButtonMask.Right);
                case 2:
                    return DirButtonMask.Right;
                case 3:
                    return (DirButtonMask.Right | DirButtonMask.Down);
                case 4:
                    return DirButtonMask.Down;
                case 5:
                    return (DirButtonMask.Down | DirButtonMask.Left);
                case 6:
                    return DirButtonMask.Left;
                case 7:
                    return (DirButtonMask.Left | DirButtonMask.Up);
            }
            return 0;
        }

        public static void DirToInputReport(int dir, ScpHidReport inputReport)
        {
            Debug.Assert(dir >= 0 && dir < 8);
            var dpadState = Parsers.HatToPad(dir);
            inputReport.Set(ButtonsEnum.Up, (dpadState & DirButtonMask.Up) != 0);
            inputReport.Set(ButtonsEnum.Right, (dpadState & DirButtonMask.Right) != 0);
            inputReport.Set(ButtonsEnum.Down, (dpadState & DirButtonMask.Down) != 0);
            inputReport.Set(ButtonsEnum.Left, (dpadState & DirButtonMask.Left) != 0);
        }
    }
}
