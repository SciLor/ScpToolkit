﻿using System;
using HidSharp;
using NativeLayer.Utilities;

namespace NativeLayer.Usb.Gamepads
{
    /// <summary>
    ///     This Gamepad is used only by the Analyzer to receive incoming HID Reports.
    /// </summary>
    internal sealed class UsbBlankGamepad : UsbGenericGamepad
    {
        private readonly DumpHelper _dumper;

        public CaptureType Capture { private get; set; }

        protected override void ParseHidReport(byte[] report)
        {
            if (Capture != CaptureType.Default)
            {
                _dumper.DumpArray(Capture.ToString(), report, report.Length);
                Capture = CaptureType.Default;
            }
        }

        #region Ctors


        public UsbBlankGamepad(HidDevice device, string header, string dumpFileName)
        {
            VendorId = (short) device.VendorID;
            ProductId = (short) device.ProductID;

            _dumper = new DumpHelper(header, dumpFileName);
        }

        //TODO: what should be here
        public override Guid DeviceClassGuid { get; }

        #endregion
    }

    public enum CaptureType
    {
        Default,
        Nothing,
        Circle,
        Cross,
        Triangle,
        Square,
        Select,
        Start,
        DpadUp,
        DpadUpAndRight,
        DpadRight,
        DpadRightAndDown,
        DpadDown,
        DpadDownAndLeft,
        DpadLeft,
        DpadLeftAndUp,
        LeftShoulder,
        RightShoulder,
        LeftTrigger,
        RightTrigger,
        LeftThumb,
        RightThumb,
        LeftStickRight,
        LeftStickLeft,
        LeftStickUp,
        LeftStickDown,
        RightStickRight,
        RightStickLeft,
        RichtStickUp,
        RightStickDown,
        Ps
    }
}