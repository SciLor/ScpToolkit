using System;
using System.Linq;
using System.Net.NetworkInformation;
using HidReport.Contract;
using HidReport.Contract.Core;
using HidReport.Contract.Enums;
using HidReport.Core;
using NativeLayer.Contract;
using NativeLayer.Usb.Usb_specification;
using NativeLayer.UsbDevices;
using NativeLayer.Utilities;

namespace OriginalDs3UsbPlugin
{
    /// <summary>
    ///     Represents a DualShock 3 controller connected via Usb.
    /// </summary>
    public sealed class UsbDs3 : WinUsbGamepadBase, IPairable
    {

        public UsbDs3(string devicePath) : base(devicePath)
        {
            try
            {
                ReadDeviceAndHostAddress();
            }
            catch (Exception e)
            {
                throw new Exception($"Couldn't request Bluetooth host  and device address for device {Dev}", e);
            }

            Log.Info($"Successfully opened device with MAC address {DeviceAddress.AsFriendlyName()}");

            CheckDeviceProtocol();

            Dev.ControlTransfer(UsbHidRequestType.HostToDevice, UsbHidRequest.SetReport, ProtocolConsts.Magic1, 0,
                ProtocolConsts.HidCommandEnable);
        }

        public PhysicalAddress HostAddress { get; private set; }
        public PhysicalAddress DeviceAddress { get; private set; }

        public void Pair(PhysicalAddress master)
        {
            var transfered = 0;
            var host = master.GetAddressBytes();
            byte[] buffer = { 0x00, 0x00, host[0], host[1], host[2], host[3], host[4], host[5] };

            //TODO:Dev.SendTransfer(UsbHidRequestType.HostToDevice, UsbHidRequest.SetReport, ProtocolConsts.HostAddress, buffer, ref transfered))

            HostAddress = master;
        }

        public override void SendFeedback(IScpFeedback feedback)
        {
            var hidReport = ProtocolConsts.HidReport;
            SetLedStatus(feedback.Pad4Lights, hidReport);
            Rumble(feedback.RumbleBig, feedback.RumbleBig, hidReport);
            Dev.ControlOut(UsbHidRequestType.HostToDevice, UsbHidRequest.SetReport,
                ToValue(UsbHidReportRequestType.Output, UsbHidReportRequestId.One), 0, hidReport);
        }

        public override Guid DeviceClassGuid { get; } = DeviceClassGuids.UsbDs3;

        protected override IScpHidReport ParseHidReport(byte[] report)
        {
            if (report[0] != 0x01)
                throw new ArgumentException("Invalid packet from controller. ID must be 1");
            PacketCounter++;

            ScpHidReport inputReport = new ScpHidReport
            {
                BatteryStatus = (DsBattery)report[30],
                PacketCounter = PacketCounter,
                ReportId = report[ProtocolConsts.Offsets.ReportId]
            };

            var buttons = (report[ProtocolConsts.Offsets.Buttons1] << 0)
            | (report[ProtocolConsts.Offsets.Buttons2] << 8)
            | (report[ProtocolConsts.Offsets.Buttons3] << 16);


            inputReport.Set(ButtonsEnum.Select, (buttons & 1 << ProtocolConsts.Ds3ButtonMaskOffset.Select) != 0);
            inputReport.Set(ButtonsEnum.L3, (buttons & 1 << ProtocolConsts.Ds3ButtonMaskOffset.L3) != 0);
            inputReport.Set(ButtonsEnum.R3, (buttons & 1 << ProtocolConsts.Ds3ButtonMaskOffset.R3) != 0);
            inputReport.Set(ButtonsEnum.Start, (buttons & 1 << ProtocolConsts.Ds3ButtonMaskOffset.Start) != 0);
            inputReport.Set(ButtonsEnum.Up, (buttons & 1 << ProtocolConsts.Ds3ButtonMaskOffset.Up) != 0);
            inputReport.Set(ButtonsEnum.Right, (buttons & 1 << ProtocolConsts.Ds3ButtonMaskOffset.Right) != 0);
            inputReport.Set(ButtonsEnum.Down, (buttons & 1 << ProtocolConsts.Ds3ButtonMaskOffset.Down) != 0);
            inputReport.Set(ButtonsEnum.Left, (buttons & 1 << ProtocolConsts.Ds3ButtonMaskOffset.Left) != 0);
            inputReport.Set(ButtonsEnum.L2, (buttons & 1 << ProtocolConsts.Ds3ButtonMaskOffset.L2) != 0);
            inputReport.Set(ButtonsEnum.R2, (buttons & 1 << ProtocolConsts.Ds3ButtonMaskOffset.R2) != 0);
            inputReport.Set(ButtonsEnum.L1, (buttons & 1 << ProtocolConsts.Ds3ButtonMaskOffset.L1) != 0);
            inputReport.Set(ButtonsEnum.R1, (buttons & 1 << ProtocolConsts.Ds3ButtonMaskOffset.R1) != 0);
            inputReport.Set(ButtonsEnum.Triangle, (buttons & 1 << ProtocolConsts.Ds3ButtonMaskOffset.Triangle) != 0);
            inputReport.Set(ButtonsEnum.Circle, (buttons & 1 << ProtocolConsts.Ds3ButtonMaskOffset.Circle) != 0);
            inputReport.Set(ButtonsEnum.Cross, (buttons & 1 << ProtocolConsts.Ds3ButtonMaskOffset.Cross) != 0);
            inputReport.Set(ButtonsEnum.Square, (buttons & 1 << ProtocolConsts.Ds3ButtonMaskOffset.Square) != 0);
            inputReport.Set(ButtonsEnum.Ps, (buttons & 1 << ProtocolConsts.Ds3ButtonMaskOffset.Ps) != 0);

            inputReport.Set(AxesEnum.Lx, report[ProtocolConsts.Offsets.AxisLx]);
            inputReport.Set(AxesEnum.Ly, report[ProtocolConsts.Offsets.AxisLy]);
            inputReport.Set(AxesEnum.Rx, report[ProtocolConsts.Offsets.AxisRx]);
            inputReport.Set(AxesEnum.Ry, report[ProtocolConsts.Offsets.AxisRy]);
            inputReport.Set(AxesEnum.Up, report[ProtocolConsts.Offsets.AxisUp]);
            inputReport.Set(AxesEnum.Right, report[ProtocolConsts.Offsets.AxisRight]);
            inputReport.Set(AxesEnum.Down, report[ProtocolConsts.Offsets.AxisDown]);
            inputReport.Set(AxesEnum.Left, report[ProtocolConsts.Offsets.AxisLeft]);
            inputReport.Set(AxesEnum.L2, report[ProtocolConsts.Offsets.AxisL2]);
            inputReport.Set(AxesEnum.R2, report[ProtocolConsts.Offsets.AxisR2]);
            inputReport.Set(AxesEnum.L1, report[ProtocolConsts.Offsets.AxisL1]);
            inputReport.Set(AxesEnum.R1, report[ProtocolConsts.Offsets.AxisR1]);
            inputReport.Set(AxesEnum.Triangle, report[ProtocolConsts.Offsets.AxisTriangle]);
            inputReport.Set(AxesEnum.Circle, report[ProtocolConsts.Offsets.AxisCircle]);
            inputReport.Set(AxesEnum.Cross, report[ProtocolConsts.Offsets.AxisCross]);
            inputReport.Set(AxesEnum.Square, report[ProtocolConsts.Offsets.AxisSquare]);
            return inputReport;
        }

        private void ReadDeviceAndHostAddress()
        {
            Dev.ControlOut(UsbHidRequestType.DeviceToHost, UsbHidRequest.GetReport, ProtocolConsts.HostAddress, 0, Buffer);
            HostAddress = new PhysicalAddress(new[]
                {Buffer[2], Buffer[3], Buffer[4], Buffer[5], Buffer[6], Buffer[7]});
            Dev.ControlOut(UsbHidRequestType.DeviceToHost, UsbHidRequest.GetReport, ProtocolConsts.DeviceAddress, 0, Buffer);
            DeviceAddress = new PhysicalAddress(new[]
                {Buffer[4], Buffer[5], Buffer[6], Buffer[7], Buffer[8], Buffer[9]});
        }


        private static void SetLedStatus(byte ledStatus, byte[] hidReport)
        {
            int ledBits = 0;
            ledBits |= ProtocolConsts.LedBitOffsets[0] & ledStatus;
            ledBits |= ProtocolConsts.LedBitOffsets[1] & ledStatus;
            ledBits |= ProtocolConsts.LedBitOffsets[2] & ledStatus;
            ledBits |= ProtocolConsts.LedBitOffsets[3] & ledStatus;
            hidReport[9] = (byte)ledBits;
        }

        /// <summary>
        ///     Send Rumble request to controller.
        /// </summary>
        private void Rumble(byte large, byte small, byte[] hidReport)
        {
            hidReport[2] = (byte)(small > 0 ? 0x01 : 0x00);
            hidReport[4] = large;
        }

        private void CheckDeviceProtocol()
        {
            if (!IniConfig.Instance.Hci.GenuineMacAddresses.Any(m => DeviceAddress.AsFriendlyName().StartsWith(m)))
            {
                var bthCompany = IniConfig.Instance.BthChipManufacturers.FirstOrDefault(
                    m =>
                        DeviceAddress.AsFriendlyName().StartsWith(m.PartialMacAddress.ToUpper()));

                if (bthCompany != null && bthCompany.Name.Equals("AirohaTechnologyCorp"))
                {
                    Model = "Fake Sony Dualshock 3 controller with Airoha Technology Corp. bluetooth chip";
                }
                else
                {
                    Model = "Fake Dualshock 3 controller with alternative protocol";
                    //NeedWorkaround = true;
                }
            }
            else
            {
                Model = "Genuine Sony DualShock 3 detected";
            }
        }
    }
}
