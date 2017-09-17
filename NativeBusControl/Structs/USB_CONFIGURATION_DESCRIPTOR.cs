using System.Runtime.InteropServices;

namespace NativeBusControl
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct USB_CONFIGURATION_DESCRIPTOR
    {
        internal byte bLength;
        internal byte bDescriptorType;
        internal ushort wTotalLength;
        internal byte bNumInterfaces;
        internal byte bConfigurationValue;
        internal byte iConfiguration;
        internal byte bmAttributes;
        internal byte MaxPower;
    }
}
