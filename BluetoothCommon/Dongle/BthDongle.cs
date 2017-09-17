using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using HidReport.Contract.Core;
using NativeLayer.Bluetooth.Ds3;
using NativeLayer.Bluetooth.Ds4;
using NativeLayer.Contract;
using NativeLayer.UsbDevices;
using NativeLayer.Utilities;

namespace NativeLayer.Bluetooth
{
    /// <summary>
    ///     Represents a Bluetooth host device.
    /// </summary>
    internal sealed partial class BthDongle : WinUsbDevice, IBluetoothDongle
    {
        #region HIDP Commandsc

        public int HID_Command(byte[] handle, byte[] channel, byte[] data)
        {
            var transfered = 0;
            var buffer = new byte[data.Length + 8];

            buffer[0] = handle[0];
            buffer[1] = handle[1];
            buffer[2] = (byte) ((data.Length + 4)%256);
            buffer[3] = (byte) ((data.Length + 4)/256);
            buffer[4] = (byte) (data.Length%256);
            buffer[5] = (byte) (data.Length/256);
            buffer[6] = channel[0];
            buffer[7] = channel[1];

            for (var i = 0; i < data.Length; i++)
                buffer[i + 8] = data[i];

            WriteBulkPipe(buffer, data.Length + 8, ref transfered);
            return transfered;
        }

        #endregion

        #region Overridden methods

        public override string ToString()
        {
            if (Initialised)
            {
                return $"Host Address : {BluetoothHostAddress.AsFriendlyName()}\n\nHCI Version  : {HciVersion}\n\nLMP Version  : {LmpVersion}";
            }
            return "Host Address : <Error>";
        }

        #endregion

        private class ConnectionList : SortedDictionary<BthHandle, BluetoothGamepadBase>
        {
        }

        #region Private fields
        private CancellationTokenSource _hciCancellationTokenSource = new CancellationTokenSource();
        private CancellationTokenSource _l2CapCancellationTokenSource = new CancellationTokenSource();
        private byte _l2CapDataIdentifier = 0x01;
        private readonly ConnectionList _connected = new ConnectionList();
        private readonly ManualResetEvent _connectionPendingEvent = new ManualResetEvent(true);
        #endregion


        #region Properties
        public PhysicalAddress BluetoothHostAddress { get; private set; }

        public string HciVersion { get; set; } = string.Empty;

        public string LmpVersion { get; set; } = string.Empty;

        public bool Initialised { get; private set; }

        #endregion

        const string genuineDs4ProductName = "Wireless Controller";

        #region Device management methods

        private BluetoothGamepadBase Add(byte lsb, byte msb, string name)
        {
            lock (_connected)
            {
                BluetoothGamepadBase connection = null;

                if (_connected.Count < 4)
                {
                    // TODO: weak check, maybe improve in future
                    if (name.Equals(genuineDs4ProductName, StringComparison.OrdinalIgnoreCase))
                        connection = new BthDs4(this, BluetoothHostAddress, lsb, msb);
                    else
                        connection = new BthDs3(this, BluetoothHostAddress, lsb, msb);

                    _connected[connection.HciHandle] = connection;
                }

                return connection;
            }
        }

        private BluetoothGamepadBase GetConnection(L2CapDataPacket packet)
        {
            lock (_connected)
            {
                return (!_connected.Any() | !_connected.ContainsKey(packet.Handle)) ? null : _connected[packet.Handle];
            }
        }

        private void Remove(byte lsb, byte msb)
        {
            lock (_connected)
            {
                var connection = new BthHandle(lsb, msb);

                if (!_connected.ContainsKey(connection))
                    return;

                _connected[connection].Stop();
                _connected.Remove(connection);
            }
        }

        #endregion

        #region Events

        public event EventHandler<ArrivalEventArgs> DeviceArrived;
        public event EventHandler<IScpHidReport> HidReportReceived;

        private bool OnDeviceArrival(IDevice arrived)
        {
            var args = new ArrivalEventArgs(arrived);

            DeviceArrived?.Invoke(this, args);

            return args.Handled;
        }

        private void OnInitialised(BluetoothGamepadBase connection)
        {
            if (OnDeviceArrival(connection))
            {
                connection.HidReportReceived += OnHidReportReceived;
                connection.Start();
            }
        }

        private void OnCompletedCount(byte lsb, byte msb, ushort count)
        {
            if (count > 0) _connected[new BthHandle(lsb, msb)].Completed();
        }

        private void OnHidReportReceived(object sender, IScpHidReport e)
        {
            HidReportReceived?.Invoke(sender, e);
        }

        #endregion
    }
}
