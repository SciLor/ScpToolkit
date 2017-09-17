//using System;
//using System.Collections.Concurrent;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Sockets;
//using System.Text;
//using System.Threading.Tasks;
//using Config;
//using ReactiveSockets;
//using RootHub.Rx;

//namespace RootHub
//{
//    class NativeFeedChannel
//    {
//        // subscribed clients who receive the native stream
//        private readonly IDictionary<int, ScpNativeFeedChannel> _nativeFeedSubscribers =
//            new ConcurrentDictionary<int, ScpNativeFeedChannel>();
//        // server to broadcast native byte stream
//        private ReactiveListener _rxFeedServer;

//        void Start()
//        {
//            _rxFeedServer = new ReactiveListener(Settings.Default.RootHubNativeFeedPort);

//            _rxFeedServer.Connections.Subscribe(socket =>
//            {
//                Log.DebugFormat("Client connected on native feed channel: {0}", socket.GetHashCode());
//                var protocol = new ScpNativeFeedChannel(socket);

//                _nativeFeedSubscribers.Add(socket.GetHashCode(), protocol);

//                protocol.Receiver.Subscribe(packet => { Log.Warn("Uuuhh how did we end up here?!"); });

//                socket.Disconnected += (sender, e) =>
//                {
//                    Log.DebugFormat(
//                        "Client disconnected from native feed channel {0}",
//                        sender.GetHashCode());

//                    _nativeFeedSubscribers.Remove(socket.GetHashCode());
//                };

//                socket.Disposed += (sender, e) =>
//                {
//                    Log.DebugFormat("Client disposed from native feed channel {0}",
//                        sender.GetHashCode());

//                    _nativeFeedSubscribers.Remove(socket.GetHashCode());
//                };
//            });

//            try
//            {
//                _rxFeedServer.Start();
//            }
//            catch (SocketException sex)
//            {
//                Log.FatalFormat("Couldn't start native feed server: {0}", sex);
//                return false;
//            }


//        }
//        /// <summary>
//        ///     Checks if the native stream is available or disabled in configuration.
//        /// </summary>
//        /// <returns>True if feed is available, false otherwise.</returns>
//        public bool IsNativeFeedAvailable()
//        {
//            return !GlobalConfiguration.Instance.DisableNative;
//        }


//        void Stop()
//        {
//            _rxFeedServer?.Dispose();
//        }
//    }
//}
