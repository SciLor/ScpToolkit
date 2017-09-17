using NativeLayer.Contract;

namespace NativeLayer.Bluetooth
{
    internal interface IBluetoothDongle
    {
        int HCI_Disconnect(BthHandle handle);
        int HID_Command(byte[] handle, byte[] channel, byte[] data);
    }
}