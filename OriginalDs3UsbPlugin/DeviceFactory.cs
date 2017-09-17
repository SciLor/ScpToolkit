using NativeLayer.Contract;
using NativeLayer.ScpBus;

namespace OriginalDs3UsbPlugin
{
    public class DeviceFactory: IUsbGamepadFactory
    {
        public string Guid { get; }
        public IUsbDevice Create(string path)
        {
            return new UsbDs3(path);
        }
    }
}
