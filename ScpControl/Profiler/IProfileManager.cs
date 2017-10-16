using System.Collections.Generic;
using System.ServiceModel;
using Profiler.Contract;
using ScpControl.Shared.Core;

namespace ScpControl.Profiler
{
    [ServiceContract]
    public interface IProfileManager
    {
        [OperationContract]
        IEnumerable<DualShockProfile> GetProfiles();

        [OperationContract]
        void SubmitProfile(DualShockProfile profile);

        [OperationContract]
        void RemoveProfile(DualShockProfile profile);
    }
}