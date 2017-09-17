using NativeLayer.Contract;

namespace NativeLayer.ScpBus
{
    public interface IUsbGamepadFactory
    {
        string Guid { get; }
        IUsbDevice Create(string path);
    }
}