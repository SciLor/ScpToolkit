using System.Runtime.InteropServices;

namespace ScpXInputBridge.XInputConstants
{
    [StructLayout(LayoutKind.Sequential)]
    public struct XINPUT_VIBRATION
    {
        public uint wLeftMotorSpeed;
        public uint wRightMotorSpeed;
    }
}