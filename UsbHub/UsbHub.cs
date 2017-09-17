using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using HidReport.Contract;
using HidSharp;
using NativeLayer.Contract;

namespace NativeLayer.ScpBus
{
    /// <summary>
    ///     Represents an Usb hub.
    /// </summary>
    public class UsbHub : DeviceHubBase
    {
        private readonly List<IUsbDevice> _devices = new List<IUsbDevice>();
        public static IList<HidDevice> LocalHidDevices => new HidDeviceLoader().GetDevices().ToList();

        private readonly UsbBusEventListener.UsbBusEventListener _usbBusEventListener =
            new UsbBusEventListener.UsbBusEventListener();

        private List<IUsbGamepadFactory> _gamepadFactories;

        public UsbHub()
        {
            _usbBusEventListener.OnDeviceArrived += OnDeviceArrived;
            InitGamepadFactories();
        }

        private void InitGamepadFactories()
        {
            //TODO: go through assemblies, find interface implementations
            _gamepadFactories = new List<IUsbGamepadFactory>();
        }


        private void OnDeviceArrived(object sender, EventArgs eventArgs)
        {
            Notified notification = Notified.Arrival;
            string Class = "";
            string path = "";
            Log.Debug($"++ Notify [{notification}] [{Class}] [{path}]");

            switch (notification)
            {
                case Notified.Arrival:
                    var dev = MadWizard.WinUSBNet.USBDevice.GetSingleDevice(path);

                    var factory = _gamepadFactories.FirstOrDefault(p => p.Guid == Class);
                    if (factory == null)
                    {
                        Log.Error($"Unknown device arrived: {dev}");
                        return;
                    }
                    IUsbDevice arrived = factory.Create(path);
                    if (arrived == null)
                    {
                        Log.Debug($"Can't init device: {dev}");
                        return;
                    }

                    Log.Info($"Device succeffullu initialized: {dev}");
                    //TODO: in root hub? arrived.HidReportReceived += OnHidReportReceived;
                    _devices.Add(arrived);
                    if (arrived is IPairable)
                    {
                        Log.Debug($"Device MAC address: {(arrived as IPairable).DeviceAddress}");
                    }
                    if (arrived is IGamepad)
                        LogArrival(arrived as IGamepad);
                    break;

                case Notified.Removal:
                    var removedDevices = _devices.Where(t => t.Path == path).ToArray();
                    Debug.Assert(removedDevices.Count() <= 1);
                    if (removedDevices.Any())
                    {
                        var device = removedDevices.First();
                        _devices.Remove(device);
                        Log.Info($"Device {device.Path} unpluged from UsbHub");
                        device.Dispose();
                    }
                    break;
            }
        }


        public override void Dispose()
        {
            _usbBusEventListener.OnDeviceArrived -= OnDeviceArrived;
            //TODO: stop listener
            foreach (var t in _devices)
                try
                {
                    t.Dispose();
                }
                catch (Exception ex)
                {
                    Log.Error($"Unexpected error {ex} with device {t.Path}");
                }
        }
    }
}