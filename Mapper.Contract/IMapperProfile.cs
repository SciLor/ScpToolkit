using HidReport.Contract.Core;
using ScpXInputBridge.XInputConstants;

namespace Mapper.Contract
{
    public interface IMapperProfile
    {
        XINPUT_GAMEPAD Map(IScpHidReport inputReport);
    }
}
