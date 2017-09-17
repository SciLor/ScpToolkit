using System;
using System.Runtime.InteropServices;

namespace NativeBusControl
{
    /// <summary>
    ///     Provides details about a single USB device
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SP_DEVICE_INTERFACE_DATA
    {
        public int cbSize;
        public Guid InterfaceClassGuid;
        public int Flags;
        public IntPtr Reserved;
    }
}