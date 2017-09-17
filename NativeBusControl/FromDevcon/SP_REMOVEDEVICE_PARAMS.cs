using System.Runtime.InteropServices;

namespace NativeBusControl.FromDevcon
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SP_REMOVEDEVICE_PARAMS
    {
        public SP_CLASSINSTALL_HEADER ClassInstallHeader;
        public int Scope;
        public int HwProfile;
    }
}