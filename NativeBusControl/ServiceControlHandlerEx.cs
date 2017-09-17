using System;

namespace NativeBusControl
{
    public delegate int ServiceControlHandlerEx(int Control, int Type, IntPtr Data, IntPtr Context);
}