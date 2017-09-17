using System.Runtime.InteropServices;

namespace ScpXInputBridge.XInputConstants
{
    [StructLayout(LayoutKind.Sequential)]
    public struct XINPUT_KEYSTROKE
    {
        public ushort VirtualKey;
        public char Unicode;
        public ushort Flags;
        public byte UserIndex;
        public byte HidCode;
    }
}