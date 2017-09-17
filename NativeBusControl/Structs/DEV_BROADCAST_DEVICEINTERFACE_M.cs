using System.Runtime.InteropServices;

namespace NativeBusControl
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class DEV_BROADCAST_DEVICEINTERFACE_M
    {
        public int dbcc_size;
        public int dbcc_devicetype;
        public int dbcc_reserved;

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 16)]
        public byte[]
            dbcc_classguid;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 255)]
        public char[] dbcc_name;
    }
}