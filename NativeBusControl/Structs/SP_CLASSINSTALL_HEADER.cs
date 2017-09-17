using System.Runtime.InteropServices;

namespace NativeBusControl
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SP_CLASSINSTALL_HEADER
    {
        public int cbSize;
        public int InstallFunction;
    }
}