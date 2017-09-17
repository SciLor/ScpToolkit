using System.Net.NetworkInformation;

namespace HidReport.Contract
{
    public interface IPairable
    {
        /// <summary>
        ///     Pairs the current device to the provided Bluetooth host.
        /// </summary>
        /// <param name="master">The MAC address of the host.</param>
        /// <returns>True on success, false otherwise.</returns>
        void Pair(PhysicalAddress master);
        PhysicalAddress HostAddress { get; }
        PhysicalAddress DeviceAddress { get; }
    }
}