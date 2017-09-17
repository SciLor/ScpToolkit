using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HidReport.Contract.Enums;

namespace OriginalDs4UsbPlugin
{
    internal static class ProtocolConsts
    {
        public static readonly byte[] _hidReport =
        {   
            0x05,
            0xFF, 0x00, 0x00, 0x00, 0x00,
            0xFF, 0xFF, 0xFF, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00,
            0x00
        };

        public static DsBattery MapBattery(byte value)
        {
            switch (value)
            {
                case 0x10:
                case 0x11:
                case 0x12:
                case 0x13:
                case 0x14:
                case 0x15:
                case 0x16:
                case 0x17:
                case 0x18:
                case 0x19:
                case 0x1A:
                    return DsBattery.Charging;
                case 0x1B:
                    return DsBattery.Charged;
            }

            return DsBattery.None;
        }

        public static class Ds4ButtonMaskOffsets
        {
            public const int Square = 4;
            public const int Cross = 5;
            public const int Circle = 6;
            public const int Triangle = 7;
            public const int L1 = 8;
            public const int R1 = 9;
            public const int L2 = 10;
            public const int R2 = 11;
            public const int Share = 12;
            public const int Options = 13;
            public const int L3 = 14;
            public const int R3 = 15;
            public const int Ps = 16;
            public const int Touchpad = 17;
        };

        //input
        public const int Buttons1 = 16;
        public const int Buttons2 = 17;
        public const int Buttons3 = 18;
        public const int Lx = 9;
        public const int Ly = 10;
        public const int Rx = 11;
        public const int Ry = 12;
        public const int L2 = 16;
        public const int R2 = 17;

        //output
        public static int R = 6; // Led Offsets
        public static int G = 7; // Led Offsets
        public static int B = 8; // Led Offsets

        public static ushort CmdHostAndDevAddress = 0x0312;
        public static ushort CmdSetHostAddress = 0x0313;

    }

}
