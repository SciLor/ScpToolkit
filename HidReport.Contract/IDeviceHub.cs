using System;
using HidReport.Contract.Core;

namespace NativeLayer.Contract
{
    public class ArrivalEventArgs : EventArgs
    {
        public ArrivalEventArgs(IGamepad device)
        {
            Device = device;
        }

        public IGamepad Device { get; private set; }
    }

    // Source of different controlles
    public interface IDeviceHub: IDisposable
    {
        event EventHandler<ArrivalEventArgs> Arrival;
        event EventHandler<IScpHidReport> Report;
    }
}
