namespace NativeBusControl
{
    enum POLICY_TYPE
    {
        SHORT_PACKET_TERMINATE = 1,
        AUTO_CLEAR_STALL = 2,
        PIPE_TRANSFER_TIMEOUT = 3,
        IGNORE_SHORT_PACKETS = 4,
        ALLOW_PARTIAL_READS = 5,
        AUTO_FLUSH = 6,
        RAW_IO = 7
    }
}