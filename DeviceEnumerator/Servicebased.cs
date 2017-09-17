using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceEnumerator
{
    class Servicebased
    {
        private ServiceControlHandlerEx _mControlHandler;

        var _serviceHandle = InteropDefenitions.RegisterServiceCtrlHandlerEx(ServiceName, _mControlHandler, IntPtr.Zero);
        private void zzz()
        {


                case SERVICE_CONTROL.DEVICEEVENT:

                    switch ((WmDeviceChangeEvent)type)
            {
                case WmDeviceChangeEvent.DBT_DEVICEARRIVAL:
                case WmDeviceChangeEvent.DBT_DEVICEREMOVECOMPLETE:

                    DEV_BROADCAST_HDR hdr;

                    hdr = Marshal.PtrToStructure<DEV_BROADCAST_HDR>(data);

                    if (hdr.dbch_devicetype == dbch_devicetype.DBT_DEVTYP_DEVICEINTERFACE)
                    {
                        DEV_BROADCAST_DEVICEINTERFACE_M deviceInterface;

                        deviceInterface = Marshal.PtrToStructure<DEV_BROADCAST_DEVICEINTERFACE_M>(data);

                        var Class = "{" + new Guid(deviceInterface.dbcc_classguid).ToString().ToUpper() + "}";

                        var path = new string(deviceInterface.dbcc_name);
                        path = path.Substring(0, path.IndexOf('\0')).ToUpper();

                        var pad = _rootHub.Notify((Notified)type, Class, path);

                        if (pad != DsPadId.None)
                        {
                            if (_rootHub.Pairable && !_rootHub.BluetoothHostAddress.Equals(_rootHub.Pads[(byte)pad].HostAddress))
                            {
                                if (_rootHub.Pads[(byte)pad].Pair(_rootHub.BluetoothHostAddress))
                                {
                                    Log.InfoFormat("Paired DualShock Device {0} to Bluetooth host {1}",
                                        _rootHub.Pads[(byte)pad].DeviceAddress, _rootHub.BluetoothHostAddress);
                                }
                                else
                                {
                                    Log.ErrorFormat("Couldn't pair DualShock Device {0} to Bluetooth host {1}",
                                        _rootHub.Pads[(byte)pad].DeviceAddress, _rootHub.BluetoothHostAddress);
                                }
                            }
                        }
                    }
                    break;
            }
            break;
        }
    }
}
