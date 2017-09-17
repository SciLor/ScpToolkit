using System;
using System.Runtime.InteropServices;

namespace NativeLayer.Driver
{
    /// <summary>
    ///     Utility class to provide native LoadLibrary() function.
    /// <remarks>Must be in it's own static class to avoid TypeLoadException.</remarks>
    /// </summary>
    public static class Kernel32
    {
        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr LoadLibrary(string librayName);
    }
}
