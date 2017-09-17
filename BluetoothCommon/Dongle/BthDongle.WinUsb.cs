using System;
using System.Threading;
using System.Threading.Tasks;
using NativeLayer.Contract;

namespace NativeLayer.Bluetooth
{
    internal sealed partial class BthDongle
    {
        public override Guid DeviceClassGuid => DeviceClassGuids.BthDongle;

        public override void Start()
        {
            if (!IsActive) return;
            Task.Factory.StartNew(HicWorker, _hciCancellationTokenSource.Token);
            Task.Factory.StartNew(L2CapWorker, _l2CapCancellationTokenSource.Token);
        }

        public override void Stop()
        {
            if (!IsActive)
                return;
            // notify tasks to stop work
            _hciCancellationTokenSource.Cancel();
            _l2CapCancellationTokenSource.Cancel();
            // reset tokens
            _hciCancellationTokenSource = new CancellationTokenSource();
            _l2CapCancellationTokenSource = new CancellationTokenSource();

            lock (_connected)
            {
                // disconnect all connected devices gracefully
                foreach (var device in _connected.Values)
                {
                    device.Disconnect();
                    device.Stop();
                }

                _connected.Clear();
            }
            base.Stop();
        }
    }
}
