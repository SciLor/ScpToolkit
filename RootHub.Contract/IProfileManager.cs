using System.Collections.Generic;
using System.ServiceModel;
using Mapper.Contract;

namespace RootHub.Contract
{
    [ServiceContract]
    public interface IProfileManager
    {
        [OperationContract]
        IEnumerable<IDualShockProfile> GetProfiles();

        [OperationContract]
        void SubmitProfile(IDualShockProfile profile);

        [OperationContract]
        void RemoveProfile(IDualShockProfile profile);
    }
}