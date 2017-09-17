using System;
using System.Net.NetworkInformation;
using System.Threading;
using Config;
using HidReport.Contract;
using HidReport.Contract.Core;
using HidReport.Contract.Enums;
using HidReport.Core;
using NativeLayer.Contract;
using NativeLayer.PacketParsers;
using NativeLayer.Usb.Usb_specification;
using NativeLayer.UsbDevices;
using OriginalDs4UsbPlugin;

namespace NativeLayer.Usb.Ds4
{
    /// <summary>
    ///     Represents a DualShock 4 controller connected via Usb.
    /// </summary>
    //DeviceClassGuids.UsbDs4
    //DualShock 4
    internal sealed class UsbDs4 : WinUsbGamepadBase, IPairable
    {
        public override Guid DeviceClassGuid { get; } = DeviceClassGuids.UsbDs4;


        private void ReadHostAndDeviceAddress()
        {
            var transfered = 0;
            //TODO:
            //if (SendTransfer(UsbHidRequestType.DeviceToHost, UsbHidRequest.GetReport, ProtocolConsts.CmdHostAndDevAddress, Buffer,
            //    ref transfered))
            {
                HostAddress =
                    new PhysicalAddress(new[]
                    {Buffer[15], Buffer[14], Buffer[13], Buffer[12], Buffer[11], Buffer[10]});

                DeviceAddress =
                    new PhysicalAddress(new[]
                    {Buffer[6], Buffer[5], Buffer[4], Buffer[3], Buffer[2], Buffer[1]});
            }
        }

        public UsbDs4(string devicePath) : base(devicePath)
        {
            ReadHostAndDeviceAddress();
            Model = "Sony Dualshock 4";
        }

        private void Rumble(byte large, byte small, byte[] hidReport)
        {
            hidReport[4] = small;
            hidReport[5] = large;
        }

        /// <summary>
        ///     Interprets a HID report sent by a DualShock 4 device.
        /// </summary>
        /// <param name="report">The HID report as byte array.</param>
        protected override IScpHidReport ParseHidReport(byte[] report)
        {
            if (report[0] != 0x01)
                throw new ArgumentException("Invalid packet from controller. ID must be 1");
            PacketCounter++;

            ScpHidReport inputReport = new ScpHidReport
            {
                PacketCounter = PacketCounter,
                BatteryStatus = ProtocolConsts.MapBattery(report[30])
            };

            //TODO: check offsets
            int buttons = (report[ProtocolConsts.Buttons1] << 0)
                | (report[ProtocolConsts.Buttons2] << 8)
                | (report[ProtocolConsts.Buttons3] << 16);
            Parsers.DirToInputReport(buttons & 0xF0, inputReport);
            var direction = buttons & 0x0F;
            Parsers.DirToInputReport(direction, inputReport);

            inputReport.Set(ButtonsEnum.Square, (buttons & 1 << ProtocolConsts.Ds4ButtonMaskOffsets.Square) != 0);
            inputReport.Set(ButtonsEnum.Cross, (buttons & 1 << ProtocolConsts.Ds4ButtonMaskOffsets.Cross) != 0);
            inputReport.Set(ButtonsEnum.Circle, (buttons & 1 << ProtocolConsts.Ds4ButtonMaskOffsets.Circle) != 0);
            inputReport.Set(ButtonsEnum.Triangle, (buttons & 1 << ProtocolConsts.Ds4ButtonMaskOffsets.Triangle) != 0);
            inputReport.Set(ButtonsEnum.L1, (buttons & 1 << ProtocolConsts.Ds4ButtonMaskOffsets.L1) != 0);
            inputReport.Set(ButtonsEnum.R1, (buttons & 1 << ProtocolConsts.Ds4ButtonMaskOffsets.R1) != 0);
            inputReport.Set(ButtonsEnum.L2, (buttons & 1 << ProtocolConsts.Ds4ButtonMaskOffsets.L2) != 0);
            inputReport.Set(ButtonsEnum.R2, (buttons & 1 << ProtocolConsts.Ds4ButtonMaskOffsets.R2) != 0);
            inputReport.Set(ButtonsEnum.Share, (buttons & 1 << ProtocolConsts.Ds4ButtonMaskOffsets.Share) != 0);
            inputReport.Set(ButtonsEnum.Options, (buttons & 1 << ProtocolConsts.Ds4ButtonMaskOffsets.Options) != 0);
            inputReport.Set(ButtonsEnum.L3, (buttons & 1 << ProtocolConsts.Ds4ButtonMaskOffsets.L3) != 0);
            inputReport.Set(ButtonsEnum.R3, (buttons & 1 << ProtocolConsts.Ds4ButtonMaskOffsets.R3) != 0);
            inputReport.Set(ButtonsEnum.Ps, (buttons & 1 << ProtocolConsts.Ds4ButtonMaskOffsets.Ps) != 0);
            inputReport.Set(ButtonsEnum.Touchpad, (buttons & 1 << ProtocolConsts.Ds4ButtonMaskOffsets.Touchpad) != 0);
            return inputReport;
        }

        public override void SendFeedback(IScpFeedback feedback)
        {
            var hidReport = ProtocolConsts._hidReport;
            Rumble(feedback.RumbleBig, feedback.RumbleSmall, hidReport);

            // enable/disable charging animation (flash)
            //_hidReport[9] = _hidReport[10] = 0x80;
            //_hidReport[9] = _hidReport[10] = 0x00;
            var transfered = 0;
            //TODO:WriteIntPipe(_hidReport, _hidReport.Length, ref transfered);
        }

        void IPairable.Pair(PhysicalAddress master)
        {
            var transfered = 0;
            var host = master.GetAddressBytes();
            byte[] buffer =
            {
                0x13, host[5], host[4], host[3], host[2], host[1], host[0], 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            };

            System.Buffer.BlockCopy(GlobalConfiguration.Instance.BdLink, 0, buffer, 7,
                GlobalConfiguration.Instance.BdLink.Length);

            SendTransfer(UsbHidRequestType.HostToDevice, UsbHidRequest.SetReport, ProtocolConsts.CmdSetHostAddress,
                buffer, ref transfered);
            HostAddress = master;

            Log.Debug($"++ Paired DS4 [{DeviceAddress}] To BTH Dongle [{HostAddress}]");
        }

        public PhysicalAddress HostAddress { get; private set; }
        public PhysicalAddress DeviceAddress { get; private set; }
    }
}