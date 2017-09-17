namespace NativeLayer.Contract
{
    public enum Notified
    {
        Ignore = 0x0000,
        Arrival = 0x8000,
        QueryRemove = 0x8001,
        Removal = 0x8004
    };
}
