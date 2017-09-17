using System.Runtime.InteropServices;

namespace ScpXInputBridge.XInputConstants
{
    [StructLayout(LayoutKind.Sequential)]
    public struct XINPUT_BATTERY_INFORMATION
    {
        public byte BatteryType;
        public byte BatteryLevel;
    }
}