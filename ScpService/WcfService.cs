using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using RootHub.Contract;

namespace RootHub
{
    class WcfService
    {
        private ServiceHost _rootHubServiceHost;

        private void StartWcfService()
        {
            var baseAddress = new Uri("net.tcp://localhost:26760/ScpRootHubService");

            var binding = new NetTcpBinding
            {
                TransferMode = TransferMode.Streamed,
                Security = new NetTcpSecurity { Mode = SecurityMode.None }
            };

            _rootHubServiceHost = new ServiceHost(this, baseAddress);
            _rootHubServiceHost.AddServiceEndpoint(typeof(IScpCommandService), binding, baseAddress);

            _rootHubServiceHost.Open();
        }

        private void Stop()
        {
            _rootHubServiceHost?.Close();

        }
    }
}
