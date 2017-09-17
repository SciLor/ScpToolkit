using System;
using System.Diagnostics;
using System.Management;
using NativeLayer.Contract;

namespace RootHub
{
    public class DeviceEventListener: IDisposable
    {

        readonly ManagementEventWatcher _watcher = new ManagementEventWatcher();
        public DeviceEventListener()
        {
            RegisterDeviceEvents();
        }

        private void RegisterDeviceEvents()
        {
            _watcher.EventArrived += WatcherOnEventArrived;
            //WqlEventQuery query = new WqlEventQuery("SELECT * FROM Win32_DeviceChangeEvent WHERE EventType = 2");
            WqlEventQuery query = new WqlEventQuery("SELECT * FROM Win32_DeviceChangeEvent");
            _watcher.Query = query;
            _watcher.Start();
            //_watcher.WaitForNextEvent();
        }

        private void UnregisterDeviceEvents()
        {
            _watcher.EventArrived -= WatcherOnEventArrived;
        }

        private void WatcherOnEventArrived(object sender, EventArrivedEventArgs eventArrivedEventArgs)
        {
            Notified type = EventTypeToNotifyEnum((UInt16) eventArrivedEventArgs.NewEvent.Properties["EventType"].Value);


            DeviceChangedEventHandler?.Invoke(this, new DeviceChangedEventArgs()
            {
                Type = type
            });

            //DEV_BROADCAST_HDR hdr = Marshal.PtrToStructure<DEV_BROADCAST_HDR>(m.LParam);
            //if (hdr.dbch_devicetype == dbch_devicetype.DBT_DEVTYP_DEVICEINTERFACE)
            //{
            //    DEV_BROADCAST_DEVICEINTERFACE_M deviceInterface = Marshal.PtrToStructure<DEV_BROADCAST_DEVICEINTERFACE_M>(m.LParam);

            //    var Class = "{" + new Guid(deviceInterface.dbcc_classguid).ToString().ToUpper() + "}";

            //    var path = new string(deviceInterface.dbcc_name);
            //    path = path.Substring(0, path.IndexOf('\0')).ToUpper();
            //}
        }

        private void TODO()
        {
            string strComputer = ".";
            ManagementObject obj = new ManagementObject();
            ManagementPath path = new ManagementPath($"winmgmts:\\\\{strComputer}\\root\\CIMV2");
            obj.Path = path;
            Trace.WriteLine(obj.ClassPath);
            //"SELECT * FROM Win32_PnPEntity WHERE Name = 'Generic PnP Monitor'",, 48)
        }

        public void Dispose()
        {
            UnregisterDeviceEvents();
            _watcher?.Dispose();
        }
    }
}
