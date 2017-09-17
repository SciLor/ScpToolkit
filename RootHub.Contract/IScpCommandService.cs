using System.Collections.Generic;
using System.ServiceModel;
using Config;
using HidReport.Contract.Enums;

namespace RootHub.Contract
{
    [ServiceContract]
    public interface IScpCommandService
    {
        //[OperationContract]
        //DualShockPadMeta GetPadDetail(DsPadId pad);

        //[OperationContract]
        //bool Rumble(DsPadId pad, byte large, byte small);

        [OperationContract]
        GlobalConfiguration RequestConfiguration();

        [OperationContract]
        void SubmitConfiguration(GlobalConfiguration configuration);

        [OperationContract]
        IEnumerable<string> GetStatusData();
    }
}