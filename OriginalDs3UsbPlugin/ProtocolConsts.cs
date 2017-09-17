
namespace OriginalDs3UsbPlugin
{
    internal static class ProtocolConsts
    {
        public static readonly byte[] HidCommandEnable = { 0x42, 0x0C, 0x00, 0x00 };

        public static readonly byte[] HidReport =
        {
            0x00, 0xFF, 0x00, 0xFF, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00,
            0xFF, 0x27, 0x10, 0x00, 0x32,
            0xFF, 0x27, 0x10, 0x00, 0x32,
            0xFF, 0x27, 0x10, 0x00, 0x32,
            0xFF, 0x27, 0x10, 0x00, 0x32,
            0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00
        };

        public static byte[] LedBitOffsets = { 0x02, 0x04, 0x08, 0x10 };

        public static int HostAddress = 0x03F5;
        public static int DeviceAddress = 0x03F2;
        public static int Magic1 = 0x03F4;

        public static class Offsets
        {
            public const int ReportId = 0;
            public const int Status = 10;

            public const int Buttons1 = 10;
            public const int Buttons2 = 11;
            public const int Buttons3 = 12;

            public const int AxisLx = 14;
            public const int AxisLy = 15;
            public const int AxisRx = 16;
            public const int AxisRy = 17;
            public const int AxisUp = 22;
            public const int AxisRight = 23;
            public const int AxisDown = 24;
            public const int AxisLeft = 25;
            public const int AxisL2 = 26;
            public const int AxisR2 = 27;
            public const int AxisL1 = 28;
            public const int AxisR1 = 29;
            public const int AxisTriangle = 30;
            public const int AxisCircle = 31;
            public const int AxisCross = 32;
            public const int AxisSquare = 33;

            public const int PlugStatus = 38;
            public const int Battery = 39;
            public const int CableStatus = 40;
        }

        /// <summary>
        ///     The offset used to identify and access the appropriate byte in <see cref="IScpHidReport"/>.
        /// </summary>
        public static class Ds3ButtonMaskOffset
        {
            public const int Select = 0;
            public const int L3 = 1;
            public const int R3 = 2;
            public const int Start = 3;
            public const int Up = 4;
            public const int Right = 5;
            public const int Down = 6;
            public const int Left = 7;
            public const int L2 = 8;
            public const int R2 = 9;
            public const int L1 = 10;
            public const int R1 = 11;
            public const int Triangle = 12;
            public const int Circle = 13;
            public const int Cross = 14;
            public const int Square = 15;
            public const int Ps = 16;
        }
    }
}
