using System;
using System.Runtime.InteropServices;

namespace NativeBusControl.Win32Usb
{
    /// <summary>
    ///     Used when registering a window to receive messages about devices added or removed from the system.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
    public class DeviceBroadcastInterface
    {
        public Guid ClassGuid;
        public int DeviceType;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)] public string Name;

        public int Reserved;
        public int Size;
    }
}