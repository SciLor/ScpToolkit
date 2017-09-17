using System;
using System.Linq;
using System.Net.NetworkInformation;
using HidReport.Contract.Enums;
using NativeLayer.Bluetooth;
using NativeLayer.Contract;
using NativeLayer.Utilities;

namespace NativeLayer.ScpBus
{
    /// <summary>
    ///     Represents a Bluetooth hub.
    /// </summary>
    public class BthHub : DeviceHubBase
    {
        private BthDongle _device;

        #region Windows messaging

        public override DsPadId Notify(Notified notification, string Class, string path)
        {
            Log.DebugFormat("++ Notify [{0}] [{1}] [{2}]", notification, Class, path);

            switch (notification)
            {
                case Notified.Arrival:
                {
                    if (_device.State != DsState.Connected)
                    {
                        var arrived = new BthDongle();

                        if (arrived.Open(path))
                        {
                            Log.DebugFormat("-- Device Arrival [{0}]", arrived.BluetoothHostAddress.AsFriendlyName());

                            _device.Close();
                            _device = arrived;

                            _device.DeviceArrived += OnDeviceArrival;
                            _device.HidReportReceived += OnHidReportReceived;

                            if (Started) _device.Start();
                            break;
                        }

                        arrived.Close();
                    }
                }
                    break;

                case Notified.Removal:

                    if (_device.Path == path)
                    {
                        Log.DebugFormat("-- Device Removal [{0}]", _device.BluetoothHostAddress.AsFriendlyName());

                        _device.Stop();
                    }
                    break;
            }

            return DsPadId.None;
        }

        #endregion

        public PhysicalAddress BluetoothHostAddress => _device.BluetoothHostAddress;

        #region Actions
        public override bool Start()
        {
            Started = true;
            string devPath = MadWizard.WinUSBNet.USBDevice.GetDevices(DeviceClassGuids.BthDongle).First().DevicePath;
            _device = new BthDongle();
            _device.DeviceArrived += OnDeviceArrival;
            _device.HidReportReceived += OnHidReportReceived;
            _device.Open(devPath);
            _device.Start();

            return Started;
        }

        public override bool Stop()
        {
            Started = false;
            //TODO: list of devices
            _device?.Stop();

            return !Started;
        }
        #endregion
    }
}
