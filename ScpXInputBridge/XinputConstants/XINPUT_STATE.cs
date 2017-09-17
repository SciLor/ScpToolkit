using System.Runtime.InteropServices;

namespace ScpXInputBridge.XInputConstants
{
    [StructLayout(LayoutKind.Sequential)]
    public struct XINPUT_STATE
    {
        public uint dwPacketNumber;
        public XINPUT_GAMEPAD Gamepad;
    }
}