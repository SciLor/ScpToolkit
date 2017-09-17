using System;

namespace NativeLayer.Contract
{
    public interface IUsbDevice: IDisposable
    {
        string Path { get; }
        int ProductId { get; }
        int VendorId { get; }
        Guid DeviceClassGuid { get; }
    }
}