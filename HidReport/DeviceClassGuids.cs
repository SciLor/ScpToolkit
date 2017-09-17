using System;

namespace NativeLayer.Contract
{
    public static class DeviceClassGuids
    {
        public static Guid BthDongle = Guid.Parse("{2F87C733-60E0-4355-8515-95D6978418B2}");

        public static Guid UsbDs3        = Guid.Parse("{E2824A09-DBAA-4407-85CA-C8E8FF5F6FFA}");
        public static Guid UsbDs4        = Guid.Parse("{2ED90CE1-376F-4982-8F7F-E056CBC3CA71}");
        public static Guid UsbAfterglow  = Guid.Parse("{7DFAADBF-4994-4EAF-B949-5032171EE421}");
        public static Guid UsbQuadStick  = Guid.Parse("{BBD873AB-10F0-4181-A963-414C14AA97B2}");
        public static Guid UsbDragonRise = Guid.Parse("{7DFAADBF-4994-4EAF-B949-5032171EE421}");
        public static Guid UsbShanWan    = Guid.Parse("{18A3A7F6-7B03-4E4A-AD08-EEF1DEC9D2E4}");
        public static Guid UsbLsiLogic   = Guid.Parse("{9C1A34A7-6DE2-4C03-B21E-DBC30BA5E358}");
        public static Guid UsbGameStopPc = Guid.Parse("{C6CD183B-65A5-4299-B408-A5B739BF353B}");
        public static Guid UsbMadcatzTournamentEdition2Fightstick = Guid.Parse("{D0BCCA4C-7477-4ABF-BDE7-AF4429DC42E6}");
        public static Guid UsbBadBigBen = Guid.Parse("");
        public static Guid UsbSnesGamedap = Guid.Parse("");
        public static Guid UsbTwinUsbJoystick = Guid.Parse("");

        public static Guid VirtualBusClass = Guid.Parse("{f679f562-3164-42ce-a4db-e7ddbe723909}");
        public static Guid VirtualBusPseudoDeviceClass = Guid.Parse("{4D36E97D-E325-11CE-BFC1-08002BE10318}");
        /// <summary>
        ///     The GUID_BTHPORT_DEVICE_INTERFACE device interface class is defined for Bluetooth radios.
        /// </summary>
        /// <remarks>https://msdn.microsoft.com/en-us/library/windows/hardware/ff545033(v=vs.85).aspx</remarks>
        // ReSharper disable once InconsistentNaming
        public static Guid GUID_BTHPORT_DEVICE_INTERFACE = Guid.Parse("{0850302A-B344-4fda-9BE9-90576B8D46F0}");
        //public static Guid HID_GUID = Guid.Parse("{A5DCBF10-6530-11D2-901F-00C04FB951ED}");

        public static Guid USBDeviceClassGuid = Guid.Parse("{88bae032-5a81-49f0-bc3d-a4ff138216d6}");
    }

}
