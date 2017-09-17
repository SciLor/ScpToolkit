using System.Runtime.InteropServices;

namespace NativeBusControl.Win32Usb
{
    /// <summary>
    ///     Access to the path for a device
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DeviceInterfaceDetailData
    {
        public int Size;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)] public string DevicePath;
    }
}