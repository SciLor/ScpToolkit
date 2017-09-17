namespace NativeBusControl
{
    public static class dbch_devicetype
    {
        //Class of devices.This structure is a DEV_BROADCAST_DEVICEINTERFACE structure.
        public const int DBT_DEVTYP_DEVICEINTERFACE = 0x00000005;

        //File system handle.This structure is a DEV_BROADCAST_HANDLE structure.
        public const int DBT_DEVTYP_HANDLE = 0x00000006;

        //OEM- or IHV-defined device type.This structure is a DEV_BROADCAST_OEM structure.
        public const int DBT_DEVTYP_OEM = 0x00000000;

        //Port device(serial or parallel). This structure is a DEV_BROADCAST_PORT structure.
        public const int DBT_DEVTYP_PORT = 0x00000003;

        //Logical volume. This structure is a DEV_BROADCAST_VOLUME structure.
        public const int DBT_DEVTYP_VOLUME = 0x00000002;
    }
}