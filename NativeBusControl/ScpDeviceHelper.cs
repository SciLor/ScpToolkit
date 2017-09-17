using System;
using System.Reflection;
using System.Runtime.InteropServices;
using log4net;

namespace NativeBusControl
{
    public static class ScpDeviceHelper
    {
        public static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static bool RegisterNotify(IntPtr form, Guid Class, ref IntPtr handle, bool window = true)
        {
            var devBroadcastDeviceInterfaceBuffer = IntPtr.Zero;

            try
            {
                var devBroadcastDeviceInterface = new DEV_BROADCAST_DEVICEINTERFACE();
                var size = Marshal.SizeOf(devBroadcastDeviceInterface);

                devBroadcastDeviceInterface.dbcc_size = size;
                devBroadcastDeviceInterface.dbcc_devicetype = dbch_devicetype.DBT_DEVTYP_DEVICEINTERFACE;
                devBroadcastDeviceInterface.dbcc_reserved = 0;
                devBroadcastDeviceInterface.dbcc_classguid = Class;

                devBroadcastDeviceInterfaceBuffer = Marshal.AllocHGlobal(size);
                Marshal.StructureToPtr(devBroadcastDeviceInterface, devBroadcastDeviceInterfaceBuffer, true);

                handle = InteropDefenitions.RegisterDeviceNotification(form, devBroadcastDeviceInterfaceBuffer,
                    window ? NativeConsts.DEVICE_NOTIFY_WINDOW_HANDLE : NativeConsts.DEVICE_NOTIFY_SERVICE_HANDLE);

                Marshal.PtrToStructure(devBroadcastDeviceInterfaceBuffer, devBroadcastDeviceInterface);

                return handle != IntPtr.Zero;
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("{0} {1}", ex.HelpLink, ex.Message);
                throw;
            }
            finally
            {
                if (devBroadcastDeviceInterfaceBuffer != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(devBroadcastDeviceInterfaceBuffer);
                }
            }
        }

        public static bool UnregisterNotify(IntPtr handle)
        {
            try
            {
                return InteropDefenitions.UnregisterDeviceNotification(handle);
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("{0} {1}", ex.HelpLink, ex.Message);
                throw;
            }
        }
    }
}
