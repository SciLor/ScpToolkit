using System;
using System.Runtime.InteropServices;
using NativeBusControl;
using NativeBusControl.FromDevcon;
using NativeBusControl.Win32Usb;

namespace NativeLayer.Driver
{
    /// <summary>
    ///     Managed wrapper for SetupAPI.
    /// </summary>
    /// <remarks>https://msdn.microsoft.com/en-us/library/windows/hardware/ff550897(v=vs.85).aspx</remarks>
    public static class Devcon
    {
        public static bool Find(Guid target, ref string path, ref string instanceId, int instance = 0)
        {
            var deviceInfoSet = IntPtr.Zero;

            try
            {
                NativeBusControl.SP_DEVICE_INTERFACE_DATA deviceInterfaceData = new NativeBusControl.SP_DEVICE_INTERFACE_DATA(), da = new NativeBusControl.SP_DEVICE_INTERFACE_DATA();
                int bufferSize = 0, memberIndex = 0;

                deviceInfoSet = Methods.SetupDiGetClassDevs(ref target, IntPtr.Zero, IntPtr.Zero,
                    NativeConsts.DIGCF_PRESENT | NativeConsts.DIGCF_DEVICEINTERFACE);

                deviceInterfaceData.cbSize = da.cbSize = Marshal.SizeOf(deviceInterfaceData);

                while (SetupApiMethods.SetupDiEnumDeviceInterfaces(deviceInfoSet, IntPtr.Zero, ref target, memberIndex,
                    ref deviceInterfaceData))
                {
                    SetupApiMethods.SetupDiGetDeviceInterfaceDetail(deviceInfoSet, ref deviceInterfaceData, IntPtr.Zero, 0,
                        ref bufferSize, ref da);
                    {
                        var detailDataBuffer = Marshal.AllocHGlobal(bufferSize);

                        Marshal.WriteInt32(detailDataBuffer,
                            (IntPtr.Size == 4) ? (4 + Marshal.SystemDefaultCharSize) : 8);

                        if (SetupApiMethods.SetupDiGetDeviceInterfaceDetail(deviceInfoSet, ref deviceInterfaceData, detailDataBuffer,
                            bufferSize, ref bufferSize, ref da))
                        {
                            var pDevicePathName = detailDataBuffer + 4;

                            path = (Marshal.PtrToStringAuto(pDevicePathName) ?? string.Empty).ToUpper();

                            if (memberIndex == instance)
                            {
                                var nBytes = 256;
                                var ptrInstanceBuf = Marshal.AllocHGlobal(nBytes);

                                SetupApiMethods.CM_Get_Device_ID(da.Flags, ptrInstanceBuf, nBytes, 0);
                                instanceId = (Marshal.PtrToStringAuto(ptrInstanceBuf) ?? string.Empty).ToUpper();

                                Marshal.FreeHGlobal(ptrInstanceBuf);
                                return true;
                            }
                        }
                        else Marshal.FreeHGlobal(detailDataBuffer);
                    }

                    memberIndex++;
                }
            }
            finally
            {
                if (deviceInfoSet != IntPtr.Zero)
                {
                    SetupApiMethods.SetupDiDestroyDeviceInfoList(deviceInfoSet);
                }
            }

            return false;
        }

        public static bool Install(string fullInfPath, ref bool rebootRequired)
        {
            return Methods.DiInstallDriver(IntPtr.Zero, fullInfPath, Consts.DIIRFLAG_FORCE_INF, ref rebootRequired);
        }

        public static bool Create(string className, Guid classGuid, string node)
        {
            var deviceInfoSet = (IntPtr)(-1);
            var deviceInfoData = new SP_DEVICE_INTERFACE_DATA();

            try
            {
                deviceInfoSet = Methods.SetupDiCreateDeviceInfoList(ref classGuid, IntPtr.Zero);

                if (deviceInfoSet == (IntPtr)(-1))
                {
                    return false;
                }

                deviceInfoData.cbSize = Marshal.SizeOf(deviceInfoData);

                if (
                    !Methods.SetupDiCreateDeviceInfo(deviceInfoSet, className, ref classGuid, null, IntPtr.Zero,
                        Consts.DICD_GENERATE_ID, ref deviceInfoData))
                {
                    return false;
                }

                if (
                    !Methods.SetupDiSetDeviceRegistryProperty(deviceInfoSet, ref deviceInfoData, Consts.SPDRP_HARDWAREID, node,
                        node.Length * 2))
                {
                    return false;
                }

                if (!Methods.SetupDiCallClassInstaller(Consts.DIF_REGISTERDEVICE, deviceInfoSet, ref deviceInfoData))
                {
                    return false;
                }
            }
            finally
            {
                if (deviceInfoSet != (IntPtr)(-1))
                {
                    Methods.SetupDiDestroyDeviceInfoList(deviceInfoSet);
                }
            }

            return true;
        }

        public static bool Remove(Guid classGuid, string path, string instanceId)
        {
            var deviceInfoSet = IntPtr.Zero;

            try
            {
                var deviceInterfaceData = new NativeBusControl.SP_DEVICE_INTERFACE_DATA();

                deviceInterfaceData.cbSize = Marshal.SizeOf(deviceInterfaceData);
                deviceInfoSet = Methods.SetupDiGetClassDevs(ref classGuid, IntPtr.Zero, IntPtr.Zero,
                    Consts.DIGCF_PRESENT | Consts.DIGCF_DEVICEINTERFACE);

                if (SetupApiMethods.SetupDiOpenDeviceInfo(deviceInfoSet, instanceId, IntPtr.Zero, 0, ref deviceInterfaceData))
                {
                    var props = new SP_REMOVEDEVICE_PARAMS();

                    props.ClassInstallHeader = new SP_CLASSINSTALL_HEADER();
                    props.ClassInstallHeader.cbSize = Marshal.SizeOf(props.ClassInstallHeader);
                    props.ClassInstallHeader.InstallFunction = Consts.DIF_REMOVE;

                    props.Scope = Consts.DI_REMOVEDEVICE_GLOBAL;
                    props.HwProfile = 0x00;

                    if (Methods.SetupDiSetClassInstallParams(deviceInfoSet, ref deviceInterfaceData, ref props,
                        Marshal.SizeOf(props)))
                    {
                        return Methods.SetupDiCallClassInstaller(Consts.DIF_REMOVE, deviceInfoSet, ref deviceInterfaceData);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (deviceInfoSet != IntPtr.Zero)
                {
                    SetupApiMethods.SetupDiDestroyDeviceInfoList(deviceInfoSet);
                }
            }

            return false;
        }

        public static bool Refresh()
        {
            UInt32 devRoot;

            if (Methods.CM_Locate_DevNode_Ex(out devRoot, IntPtr.Zero, 0, IntPtr.Zero) != Consts.CR_SUCCESS) return false;
            return Methods.CM_Reenumerate_DevNode_Ex(devRoot, 0, IntPtr.Zero) == Consts.CR_SUCCESS;
        }
    }
}