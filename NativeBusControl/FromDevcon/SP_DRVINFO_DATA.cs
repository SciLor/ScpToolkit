using System;
using System.Runtime.InteropServices;

namespace NativeBusControl.FromDevcon
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SP_DRVINFO_DATA
    {
        public UInt32 cbSize;
        public UInt32 DriverType;
        public IntPtr Reserved;
        public string Description;
        public string MfgName;
        public string ProviderName;
        public DateTime DriverDate;
        public UInt64 DriverVersion;
    }
}