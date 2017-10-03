﻿using System.Collections.Generic;
using System.ServiceModel;
using ScpControl.Profiler;
using ScpControl.ScpCore;
using ScpControl.Shared.Core;

namespace ScpControl.Wcf
{
    [ServiceContract]
    public interface IScpCommandService: IProfileManager
    {
        [OperationContract]
        bool IsNativeFeedAvailable();

        [OperationContract]
        DualShockPadMeta GetPadDetail(DsPadId pad);

        [OperationContract]
        bool Rumble(DsPadId pad, byte large, byte small);

        [OperationContract]
        GlobalConfiguration RequestConfiguration();

        [OperationContract]
        void SubmitConfiguration(GlobalConfiguration configuration);

        [OperationContract]
        IEnumerable<string> GetStatusData();

        [OperationContract]
        void PromotePad(byte pad);
    }
}