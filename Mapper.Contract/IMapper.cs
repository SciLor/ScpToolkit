using HidReport.Contract.Core;

namespace Mapper.Contract
{
    public interface IMapper
    {
        void OnHidreportReceived(object sender, IScpHidReport e);
    }
}