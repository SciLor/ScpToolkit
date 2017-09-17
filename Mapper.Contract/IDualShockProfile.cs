using HidReport.Contract.Core;

namespace Mapper.Contract
{
    public interface IDualShockProfile
    {
        void Remap(IScpHidReport report);
    }
}