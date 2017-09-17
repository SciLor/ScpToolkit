using System;
using System.Threading.Tasks;
using HidReport.Contract.Core;
using ReactiveSockets;

namespace RootHub.Rx
{
    public class ScpNativeFeedChannel : IChannel<IScpHidReport>
    {
        private readonly IReactiveSocket _socket;

        /// <summary>
        /// Initializes the channel with the given socket, using 
        /// the given encoding for messages.
        /// </summary>
        public ScpNativeFeedChannel(IReactiveSocket socket)
        {
            this._socket = socket;

            //var packetLen = 0; //TODO: ScpHidReportFactory.Length
            //Receiver = from packet in socket.Receiver.Buffer(packetLen)
            //           select packet.ToArray();
            Receiver = null;
        }

        public IObservable<IScpHidReport> Receiver { get; private set; }

        public Task SendAsync(IScpHidReport message)
        {
            try
            {
                return _socket.SendAsync(null); //TODO: message);
            }
            catch (InvalidOperationException)
            {
                return Task.FromResult<object>(null);
            }
        }
    }
}