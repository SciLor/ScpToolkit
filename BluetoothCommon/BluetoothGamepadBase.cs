using System;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Config;
using HidReport.Contract.Core;
using HidReport.Core;
using NativeLayer.Contract;
using NativeLayer.Utilities;

namespace NativeLayer.Bluetooth
{
    /// <summary>
    ///     Represents a generic Bluetooth client device.
    /// </summary>
    internal abstract class BluetoothGamepadBase : BluetoothDevice, IGamepad
    {
        #region Private fields

        //TODO: private readonly IObservable<long> _outputReportSchedule = Observable.Interval(TimeSpan.FromMilliseconds(10), Scheduler.Default);

        private IDisposable _outputReportTask;

        private readonly TaskQueue _inputReportQueue = new TaskQueue();

        #endregion

        #region Protected fields

        protected bool Blocked, IsIdle = true, IsDisconnect;
        protected byte CableStatus = 0;
        protected readonly IBluetoothDongle BluetoothDongle;
        protected byte Init = 0;

        protected DateTime Last = DateTime.Now;

        protected DateTime Idle = DateTime.Now;

        protected DateTime Tick = DateTime.Now;

        protected DateTime DisconnectTime = DateTime.Now;

        protected uint Packet;
        protected byte PlugStatus = 0;
        protected uint Queued = 0;
        protected ScpHidReport HidReport = new ScpHidReport();

        private bool _publish;

        #endregion

        #region Public properties
        public string Model { get; } = "Bluetooth gamepad";

        #endregion

        #region Public methods

        public virtual void Start()
        {
            //TODO: _outputReportTask = _outputReportSchedule.Subscribe(tick => OnTimer());
        }

        public virtual bool Disconnect()
        {
            _publish = false;
            return BluetoothDongle.HCI_Disconnect(HciHandle) > 0;
        }

        public virtual void Stop()
        {
            _outputReportTask?.Dispose();
            Packet = 0;
            _publish = false;
            OnHidReportReceived(HidReport);
        }

        public virtual void Close()
        {
            Stop();
        }

        public abstract void ParseHidReport(byte[] report);

        public virtual bool InitHidReport(byte[] report)
        {
            return true;
        }

        public override string ToString()
        {
            return $"BluetoothGamepad: {DeviceAddress.AsFriendlyName()} - {Packet:X8}";
        }

        public virtual void Completed()
        {
            lock (this)
            {
                Blocked = false;
            }
        }

        #endregion

        #region Events

        public event EventHandler<ScpHidReport> HidReportReceived;

        protected void OnHidReportReceived(ScpHidReport report)
        {
            if (GlobalConfiguration.Instance.UseAsyncHidReportProcessing)
            {
                _inputReportQueue.Enqueue(() => Task.Run(() =>
                {
                    HidReportReceived?.Invoke(this, report);
                }));
            }
            else
            {
                HidReportReceived?.Invoke(this, report);
            }
        }

        #endregion

        #region Protected methods

        protected virtual void Process(DateTime now)
        {
        }

        private void OnTimer()
        {
            #region Calculate and trigger idle auto-disconnect

            var now = DateTime.Now;

            if (IsIdle && GlobalConfiguration.Instance.IdleDisconnect)
            {
                if ((now - Idle).TotalMilliseconds >= GlobalConfiguration.Instance.IdleTimeout)
                {
                    Log.Info($"Bluetooth pad {ToString()} disconnected due to idle timeout");

                    IsDisconnect = false;
                    IsIdle = false;

                    Disconnect();
                    return;
                }
            }
            else if (IsDisconnect)
            {
                if ((now - DisconnectTime).TotalMilliseconds >= 2000)
                {
                    Log.Info($"Bluetooth pad {ToString()} disconnected due to quick disconnect combo");

                    IsDisconnect = false;
                    IsIdle = false;

                    Disconnect();
                    return;
                }
            }

            #endregion

            Process(now);
        }

        #endregion

        #region Ctors
        protected BluetoothGamepadBase(IBluetoothDongle device, byte lsb, byte msb)
            : base(new BthHandle(lsb, msb))
        {
            BluetoothDongle = device;
        }

        public event EventHandler<IScpHidReport> HidReportEventHandler;

        public abstract void SendFeedback(IScpFeedback feedback);

        #endregion
    }
}