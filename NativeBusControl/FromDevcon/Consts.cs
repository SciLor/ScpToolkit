

// ReSharper disable InconsistentNaming

namespace NativeBusControl.FromDevcon
{
    public static class Consts
    {
        public const int DIGCF_PRESENT = 0x0002;
        public const int DIGCF_DEVICEINTERFACE = 0x0010;

        public const int DICD_GENERATE_ID = 0x0001;
        public const int SPDRP_HARDWAREID = 0x0001;

        public const int DIF_REMOVE = 0x0005;
        public const int DIF_REGISTERDEVICE = 0x0019;

        public const int DI_REMOVEDEVICE_GLOBAL = 0x0001;


        public const uint CM_REENUMERATE_NORMAL = 0x00000000;
        public const uint CM_REENUMERATE_SYNCHRONOUS = 0x00000001;
        // XP and later versions 
        public const uint CM_REENUMERATE_RETRY_INSTALLATION = 0x00000002;
        public const uint CM_REENUMERATE_ASYNCHRONOUS = 0x00000004;

        public const uint CR_SUCCESS = 0x00000000;

        public const uint DIIRFLAG_FORCE_INF = 0x00000002;

    }
}
