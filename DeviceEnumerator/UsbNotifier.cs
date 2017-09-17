using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using NativeBusControl;
using NativeBusControl.Win32Usb;

namespace NativeLayer.Usb.PnP
{
    /// <summary>
    ///     A modified version of the USB HID Component for C#.
    ///     <remarks>http://www.codeproject.com/Articles/18099/A-USB-HID-Component-for-C</remarks>
    /// </summary>
    public class UsbNotifier
    {
        /// <summary>
        ///     The product id from the USB device you want to use.
        /// </summary>
        public ushort ProductId { get; set; }

        /// <summary>
        ///     The vendor id from the USB device you want to use.
        /// </summary>
        public ushort VendorId { get; set; }

        /// <summary>
        ///     The device class GUID this notifier will listen for. Defaults to <see cref="Win32Usb.HidGuid" />.
        /// </summary>
        public Guid ClassGuid { get; private set; }

        //events
        /// <summary>
        ///     This event will be triggered when the device you specified is pluged into your usb port on
        ///     the computer. And it is completly enumerated by windows and ready for use.
        /// </summary>
        [Description(
            "The event that occurs when a usb hid device with the specified vendor id and product id is found on the bus"
            )]
        [Category("Embedded Event")]
        [DisplayName("OnSpecifiedDeviceArrived")]
        public event EventHandler OnSpecifiedDeviceArrived;

        /// <summary>
        ///     This event will be triggered when the device you specified is removed from your computer.
        /// </summary>
        [Description(
            "The event that occurs when a usb hid device with the specified vendor id and product id is removed from the bus"
            )]
        [Category("Embedded Event")]
        [DisplayName("OnSpecifiedDeviceRemoved")]
        public event EventHandler OnSpecifiedDeviceRemoved;

        /// <summary>
        ///     This event will be triggered when a device is pluged into your usb port on
        ///     the computer. And it is completly enumerated by windows and ready for use.
        /// </summary>
        [Description("The event that occurs when a usb hid device is found on the bus")]
        [Category("Embedded Event")]
        [DisplayName("OnDeviceArrived")]
        public event EventHandler OnDeviceArrived;

        /// <summary>
        ///     This event will be triggered when a device is removed from your computer.
        /// </summary>
        [Description("The event that occurs when a usb hid device is removed from the bus")]
        [Category("Embedded Event")]
        [DisplayName("OnDeviceRemoved")]
        public event EventHandler OnDeviceRemoved;

        /// <summary>
        ///     Registers this application, so it will be notified for usb events.
        /// </summary>
        /// <param name="handle">a IntPtr, that is a handle to the application.</param>
        /// <example>
        ///     This sample shows how to implement this method in your form.
        ///     <code> 
        /// protected override void OnHandleCreated(EventArgs e)
        /// {
        ///     base.OnHandleCreated(e);
        ///     usb.RegisterHandle(Handle);
        /// }
        /// </code>
        /// </example>
        public void RegisterHandle(IntPtr handle)
        {
            _usbEventHandle = RegisterForUsbEvents(handle, ClassGuid);

            if (_usbEventHandle == IntPtr.Zero)
            {
                throw new InvalidOperationException("Couldn't register for receiving USB events");
            }

            _handle = handle;
            //Check if the device is already present.
            CheckDevicePresent();
        }

        /// <summary>
        ///     Unregisters this application, so it won't be notified for usb events.
        /// </summary>
        /// <returns>Returns if it wass succesfull to unregister.</returns>
        public bool UnregisterHandle()
        {
            return UnregisterForUsbEvents(_handle);
        }

        /// <summary>
        ///     This method will filter the messages that are passed for usb device change messages only.
        ///     And parse them and take the appropriate action
        /// </summary>
        /// <param name="msg">The window message received by the window procedure.</param>
        /// <param name="wParam">The wParam argument.</param>
        public void ParseMessages(int msg, IntPtr wParam)
        {
            if (msg != NativeConsts.WM_DEVICECHANGE) return;

            switch (wParam.ToInt32()) // Check the W parameter to see if a device was inserted or removed
            {
                case Constants.DEVICE_ARRIVAL: // inserted
                    if (OnDeviceArrived != null)
                    {
                        OnDeviceArrived(this, new EventArgs());
                    }
                    CheckDevicePresent();
                    break;
                case Constants.DEVICE_REMOVECOMPLETE: // removed
                    if (OnDeviceRemoved != null)
                    {
                        OnDeviceRemoved(this, new EventArgs());
                    }
                    CheckDevicePresent();
                    break;
            }
        }

        /// <summary>
        ///     Checks the devices that are present at the moment and checks if one of those
        ///     is the device you defined by filling in the product id and vendor id.
        /// </summary>
        public void CheckDevicePresent()
        {
            var specifiedDevice = FindDevice(VendorId, ProductId); // look for the device on the USB bus
            if (specifiedDevice) // did we find it?
            {
                if (OnSpecifiedDeviceArrived == null) return;

                OnSpecifiedDeviceArrived(this, new EventArgs());
            }
            else
            {
                if (OnSpecifiedDeviceRemoved != null)
                {
                    OnSpecifiedDeviceRemoved(this, new EventArgs());
                }
            }
        }

        /// <summary>
        ///     Helper method to return the device path given a DeviceInterfaceData structure and an InfoSet handle.
        ///     Used in 'FindDevice' so check that method out to see how to get an InfoSet handle and a DeviceInterfaceData.
        /// </summary>
        /// <param name="hInfoSet">Handle to the InfoSet</param>
        /// <param name="oInterface">DeviceInterfaceData structure</param>
        /// <returns>The device path or null if there was some problem</returns>
        private static string GetDevicePath(IntPtr hInfoSet, ref SP_DEVICE_INTERFACE_DATA oInterface)
        {
            uint nRequiredSize = 0;
            // Get the device interface details
            if (
                !SetupApiMethods.SetupDiGetDeviceInterfaceDetail(hInfoSet, ref oInterface, IntPtr.Zero, 0, ref nRequiredSize,
                    IntPtr.Zero))
            {
                var detailDataBuffer = Marshal.AllocHGlobal((int) nRequiredSize);
                Marshal.WriteInt32(detailDataBuffer, IntPtr.Size == 4 ? 4 + Marshal.SystemDefaultCharSize : 8);

                try
                {
                    if (SetupApiMethods.SetupDiGetDeviceInterfaceDetail(hInfoSet, ref oInterface, detailDataBuffer, nRequiredSize,
                        ref nRequiredSize, IntPtr.Zero))
                    {
                        var pDevicePathName = new IntPtr(detailDataBuffer.ToInt64() + 4);
                        return Marshal.PtrToStringAnsi(pDevicePathName) ?? string.Empty;
                    }
                }
                finally
                {
                    Marshal.FreeHGlobal(detailDataBuffer);
                }
            }

            return string.Empty;
        }

        /// <summary>
        ///     Finds a device given its PID and VID
        /// </summary>
        private bool FindDevice(int nVid, int nPid)
        {
            var strSearch = $"vid_{nVid:x4}&pid_{nPid:x4}"; // first, build the path search string
            var gHid = ClassGuid; // next, get the GUID from Windows that it uses to represent the HID USB interface
            var hInfoSet = SetupApiMethods.SetupDiGetClassDevs(ref gHid, null, IntPtr.Zero, NativeConsts.DIGCF_DEVICEINTERFACE | NativeConsts.DIGCF_PRESENT);
            // this gets a list of all HID devices currently connected to the computer (InfoSet)

            try
            {
                var oInterface = new SP_DEVICE_INTERFACE_DATA(); // build up a device interface data block
                //TODO: oInterface.Size = Marshal.SizeOf(oInterface);
                // Now iterate through the InfoSet memory block assigned within Windows in the call to SetupDiGetClassDevs
                // to get device details for each device connected
                var nIndex = 0;
                while (SetupApiMethods.SetupDiEnumDeviceInterfaces(hInfoSet, 0, ref gHid, (uint) nIndex, ref oInterface))
                    // this gets the device interface information for a device at index 'nIndex' in the memory block
                {
                    var strDevicePath = GetDevicePath(hInfoSet, ref oInterface);
                    // get the device path (see helper method 'GetDevicePath')
                    if (strDevicePath.IndexOf(strSearch, StringComparison.Ordinal) >= 0)
                        // do a string search, if we find the VID/PID string then we found our device!
                    {
                        return true;
                    }
                    nIndex++; // if we get here, we didn't find our device. So move on to the next one.
                }
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                // Before we go, we have to free up the InfoSet memory reserved by SetupDiGetClassDevs
                SetupApiMethods.SetupDiDestroyDeviceInfoList(hInfoSet);
            }
            return false; // oops, didn't find our device
        }

        #region Private fields

        private IntPtr _handle;
        private IntPtr _usbEventHandle;

        #endregion

        #region Ctors

        public UsbNotifier()
        {
            ClassGuid = HidGuid;
        }

        /// <summary>
        /// </summary>
        /// <param name="classGuid">GUID that specifies the device interface class.</param>
        public UsbNotifier(Guid classGuid)
        {
            ClassGuid = classGuid;
        }

        /// <summary>
        /// </summary>
        /// <param name="vid">Vendor identifier.</param>
        /// <param name="pid">Product identifier.</param>
        public UsbNotifier(ushort vid, ushort pid) : this()
        {
            VendorId = vid;
            ProductId = pid;
        }

        /// <summary>
        /// </summary>
        /// <param name="vid">Vendor identifier.</param>
        /// <param name="pid">Product identifier.</param>
        /// <param name="classGuid">GUID that specifies the device interface class.</param>
        public UsbNotifier(ushort vid, ushort pid, Guid classGuid) : this(vid, pid)
        {
            ClassGuid = classGuid;
        }


        /// <summary>
        ///     Registers a window to receive windows messages when a device is inserted/removed. Need to call this
        ///     from a form when its handle has been created, not in the form constructor. Use form's OnHandleCreated override.
        /// </summary>
        /// <param name="hWnd">Handle to window that will receive messages</param>
        /// <param name="gClass">Class of devices to get messages for</param>
        /// <returns>A handle used when unregistering</returns>
        protected static IntPtr RegisterForUsbEvents(IntPtr hWnd, Guid gClass)
        {
            var retVal = IntPtr.Zero;

            return ScpDeviceHelper.RegisterNotify(hWnd, gClass, ref retVal) ? retVal : IntPtr.Zero;
        }

        /// <summary>
        ///     Unregisters notifications. Can be used in form dispose
        /// </summary>
        /// <param name="hHandle">Handle returned from RegisterForUSBEvents</param>
        /// <returns>True if successful</returns>
        protected static bool UnregisterForUsbEvents(IntPtr hHandle)
        {
            return ScpDeviceHelper.UnregisterNotify(hHandle);
        }

        /// <summary>
        ///     Helper to get the HID guid.
        /// </summary>
        protected static Guid HidGuid
        {
            get
            {
                Guid gHid;
                SetupApiMethods.HidD_GetHidGuid(out gHid);
                return gHid;
            }
        }
        #endregion
    }
}