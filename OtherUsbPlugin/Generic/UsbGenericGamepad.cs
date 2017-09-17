using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using HidSharp;
using NativeLayer.Utilities;

namespace NativeLayer.Usb.Gamepads
{
    internal abstract class UsbGenericGamepad : WinUsbGamepadBase
    {
        #region Private fields

        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private HidDevice _currentHidDevice;
        private HidStream _currentHidStream;

        #endregion

        #region Actions
        public override bool Open(string devicePath)
        {
            short vid, pid;

            GetHardwareId(devicePath, out vid, out pid);

            var loader = new HidDeviceLoader();

            // search for HID
            _currentHidDevice = loader.GetDevices(vid, pid).FirstOrDefault();

            if (_currentHidDevice == null)
            {
                Log.ErrorFormat("Couldn't find device with VID: {0}, PID: {1}",
                    vid, pid);
                return false;
            }

            // open HID
            if (!_currentHidDevice.TryOpen(out _currentHidStream))
            {
                Log.ErrorFormat("Couldn't open device {0}", _currentHidDevice);
                return false;
            }

            IsActive = true;
            Path = devicePath;

            return IsActive;
        }

        public override bool Start()
        {
            // TODO: remove duplicate code
            if (!IsActive) return State == DsState.Connected;

            State = DsState.Connected;
            PacketCounter = 0;

            Task.Factory.StartNew(MainHidInputReader, _cancellationTokenSource.Token);

            return State == DsState.Connected;
        }


        public override bool Stop()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();

            // pad reservation not supported
            State = DsState.Disconnected;

            return true;
        }

        #endregion

        #region Main worker

        private void MainHidInputReader(object o)
        {
            var token = (CancellationToken) o;
            var buffer = new byte[_currentHidDevice.MaxInputReportLength];

            while (!token.IsCancellationRequested)
            {
                var count = _currentHidStream.Read(buffer, 0, buffer.Length);

                if (count > 0)
                {
                    ParseHidReport(buffer);
                }
            }
        }

        #endregion

        #region Unused methods

        protected override void Process(DateTime now)
        {
            // ignore
        }

        public override bool Rumble(byte large, byte small)
        {
            return true; // ignore
        }
        #endregion
    }
}
