using System.Runtime.InteropServices;

namespace NativeBusControl
{
    [StructLayout(LayoutKind.Sequential)]
    public class DEV_BROADCAST_HDR
    {
        public int dbch_size;
        public int dbch_devicetype;
        public int dbch_reserved;
    }
}