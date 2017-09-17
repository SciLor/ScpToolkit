using System;
// ReSharper disable InconsistentNaming

namespace NativeBusControl.Win32Usb
{
    public static class Constants
    {
        /// <summary>WParam for above : A device was inserted</summary>
        public const int DEVICE_ARRIVAL = 0x8000;

        /// <summary>WParam for above : A device was removed</summary>
        public const int DEVICE_REMOVECOMPLETE = 0x8004;

        /// <summary>Used when registering for device insert/remove messages : specifies the type of device</summary>
        public const int DEVTYP_DEVICEINTERFACE = 0x05;

        /// <summary>Used when registering for device insert/remove messages : we're giving the API call a window handle</summary>
        public const int DEVICE_NOTIFY_WINDOW_HANDLE = 0;

        /// <summary>Purges Win32 transmit buffer by aborting the current transmission.</summary>
        public const uint PURGE_TXABORT = 0x01;

        /// <summary>Purges Win32 receive buffer by aborting the current receive.</summary>
        public const uint PURGE_RXABORT = 0x02;

        /// <summary>Purges Win32 transmit buffer by clearing it.</summary>
        public const uint PURGE_TXCLEAR = 0x04;

        /// <summary>Purges Win32 receive buffer by clearing it.</summary>
        public const uint PURGE_RXCLEAR = 0x08;

        /// <summary>CreateFile : Open file for read</summary>
        public const uint GENERIC_READ = 0x80000000;

        /// <summary>CreateFile : Open file for write</summary>
        public const uint GENERIC_WRITE = 0x40000000;

        /// <summary>CreateFile : file share for write</summary>
        public const uint FILE_SHARE_WRITE = 0x2;

        /// <summary>CreateFile : file share for read</summary>
        public const uint FILE_SHARE_READ = 0x1;

        /// <summary>CreateFile : Open handle for overlapped operations</summary>
        public const uint FILE_FLAG_OVERLAPPED = 0x40000000;

        /// <summary>CreateFile : Resource to be "created" must exist</summary>
        public const uint OPEN_EXISTING = 3;

        /// <summary>CreateFile : Resource will be "created" or existing will be used</summary>
        public const uint OPEN_ALWAYS = 4;

        /// <summary>ReadFile/WriteFile : Overlapped operation is incomplete.</summary>
        public const uint ERROR_IO_PENDING = 997;

        /// <summary>Infinite timeout</summary>
        public const uint INFINITE = 0xFFFFFFFF;

        /// <summary>
        ///     Simple representation of a null handle : a closed stream will get this handle. Note it is public for
        ///     comparison by higher level classes.
        /// </summary>
        public static IntPtr NullHandle = IntPtr.Zero;

        /// <summary>Simple representation of the handle returned when CreateFile fails.</summary>
        public static IntPtr InvalidHandleValue = new IntPtr(-1);



    }
}