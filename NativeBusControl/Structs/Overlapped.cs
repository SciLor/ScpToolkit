using System;
using System.Runtime.InteropServices;

namespace NativeBusControl.Win32Usb
{

    /// <summary>
    ///     An overlapped structure used for overlapped IO operations. The structure is
    ///     only used by the OS to keep state on pending operations. You don't need to fill anything in if you
    ///     unless you want a Windows event to fire when the operation is complete.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Overlapped
    {
        public uint Internal;
        public uint InternalHigh;
        public uint Offset;
        public uint OffsetHigh;
        public IntPtr Event;
    }
}
