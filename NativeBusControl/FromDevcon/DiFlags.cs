using System;

namespace NativeBusControl.FromDevcon
{
    [Flags]
    public enum DiFlags : uint
    {
        DIIDFLAG_SHOWSEARCHUI = 1,
        DIIDFLAG_NOFINISHINSTALLUI = 2,
        DIIDFLAG_INSTALLNULLDRIVER = 3
    }
}