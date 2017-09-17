using System.Net.NetworkInformation;

namespace HidReport.Contract
{
    public interface IPairingMaster
    {
        PhysicalAddress HostAddress { get; }
    }
}