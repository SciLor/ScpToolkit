namespace NativeBusControl
{
    public static class NativeConsts
    {
        public const int PBT_APMRESUMEAUTOMATIC = 0x0012;
        public const int PBT_APMSUSPEND = 0x0004;

        public const int DEVICE_NOTIFY_WINDOW_HANDLE = 0x0000;
        public const int DEVICE_NOTIFY_SERVICE_HANDLE = 0x0001;
        public const int DEVICE_NOTIFY_ALL_INTERFACE_CLASSES = 0x0004;

        /// <summary>Windows message sent when a device is inserted or removed</summary>
        public const int WM_DEVICECHANGE = 0x0219;

        /// <summary>Used in SetupDiClassDevs to get devices present in the system</summary>
        public const int DIGCF_PRESENT = 0x0002;
        /// <summary>Used in SetupDiClassDevs to get device interface details</summary>
        public const int DIGCF_DEVICEINTERFACE = 0x0010;

        public const uint FILE_ATTRIBUTE_NORMAL = 0x80;
        public const uint FILE_FLAG_OVERLAPPED = 0x40000000;
        public const uint FILE_SHARE_READ = 1;
        public const uint FILE_SHARE_WRITE = 2;
        public const uint GENERIC_READ = 0x80000000;
        public const uint GENERIC_WRITE = 0x40000000;
        public const int INVALID_HANDLE_VALUE = -1;
        public const uint OPEN_EXISTING = 3;
        public const uint DEVICE_SPEED = 1;
        public const byte USB_ENDPOINT_DIRECTION_MASK = 0x80;

        public const int DIF_PROPERTYCHANGE = 0x12;
        public const int DICS_ENABLE = 1;
        public const int DICS_DISABLE = 2;
        public const int DICS_PROPCHANGE = 3;
        public const int DICS_FLAG_GLOBAL = 1;

    }



}
