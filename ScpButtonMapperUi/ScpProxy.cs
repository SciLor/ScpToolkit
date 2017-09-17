using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reflection;
using System.ServiceModel;
using Config;
using HidReport.Contract.Core;
using HidReport.Contract.Enums;
using log4net;
using Mapper.Contract;
using ReactiveSockets;
using RootHub.Contract;
using RootHub.Rx;
using ScpProfiler.Properties;

namespace ScpProfiler
{
    public sealed partial class ScpProxy : Component
    {
        #region XInput extensions

        private SCP_EXTN TranslateAndWrapInformationPs3(IScpHidReport inputReport)
        {

            return new SCP_EXTN
            {
                SCP_UP     = inputReport[AxesEnum.Up].Pressure,
                SCP_RIGHT  = inputReport[AxesEnum.Right].Pressure,
                SCP_DOWN   = inputReport[AxesEnum.Down].Pressure,
                SCP_LEFT   = inputReport[AxesEnum.Left].Pressure,
                SCP_LX     = inputReport[AxesEnum.Lx].Axis,
                SCP_LY     = -inputReport[AxesEnum.Ly].Axis,
                SCP_L1     = inputReport[AxesEnum.L1].Pressure,
                SCP_L2     = inputReport[AxesEnum.L2].Pressure,
                SCP_L3     = inputReport[ButtonsEnum.L3].Pressure,
                SCP_RX     = inputReport[AxesEnum.Rx].Axis,
                SCP_RY     = -inputReport[AxesEnum.Ry].Axis,
                SCP_R1     = inputReport[AxesEnum.R1].Pressure,
                SCP_R2     = inputReport[AxesEnum.R2].Pressure,
                SCP_R3     = inputReport[ButtonsEnum.R3].Pressure,
                SCP_T      = inputReport[AxesEnum.Triangle].Pressure,
                SCP_C      = inputReport[AxesEnum.Circle].Pressure,
                SCP_X      = inputReport[AxesEnum.Cross].Pressure,
                SCP_S      = inputReport[AxesEnum.Square].Pressure,
                SCP_SELECT = inputReport[ButtonsEnum.Select].Pressure,
                SCP_START  = inputReport[ButtonsEnum.Start].Pressure,
                SCP_PS     = inputReport[ButtonsEnum.Ps].Pressure
                //TODO:
                //SCP_SELECT = inputReport[ButtonsEnum.Share].Pressure,
                //SCP_START = inputReport[ButtonsEnum.Options].Pressure,
            };

        }

        private SCP_EXTN TranslateAndWrapInformationPs4(IScpHidReport inputReport)
        {
            return new SCP_EXTN
            {

            };
        }
        /// <summary>
        ///     Used by ScpXInputBridge to request pressure sensitive button information.
        /// </summary>
        /// <param name="dwUserIndex">The pad index to request data from (zero-based).</param>
        /// <returns>The pressure sensitive button/axis information.</returns>
        public SCP_EXTN GetExtended(uint dwUserIndex)
        {
            IScpHidReport inputReport;
            var extended = default(SCP_EXTN);

            try
            {
                inputReport = _packetCache[(DsPadId) dwUserIndex];
            }
            catch (KeyNotFoundException)
            {
                return extended;
            }
            //TODO:if(inputReport is ScpHidReport)
            return TranslateAndWrapInformationPs3(inputReport);
            //else if (inputReport is ScpHidReport)
            //    return TranslateAndWrapInformationPs4((ScpHidReport)inputReport);
        }

        public IScpHidReport GetReport(uint dwUserIndex)
        {
            try
            {
                return _packetCache[(DsPadId) dwUserIndex];
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
        }

        #endregion

        #region Private fields

        private readonly ReactiveClient _rxFeedClient = new ReactiveClient(Settings.Default.RootHubNativeFeedHost,
            Settings.Default.RootHubNativeFeedPort);

        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // caches the latest HID report for every pad in a thread-save dictionary
        private readonly IDictionary<DsPadId, IScpHidReport> _packetCache =
            new ConcurrentDictionary<DsPadId, IScpHidReport>();

        private IScpCommandService _rootHub;
        private IProfileManager _profileManager;

        #endregion

        #region Public properties

        public bool IsActive { get; private set; }

        public IList<string> StatusData => _rootHub.GetStatusData().ToList();

        #endregion

        #region WCF methods

        public void PromotePad(byte pad)
        {
            _rootHub.PromotePad(pad);
        }

        public GlobalConfiguration ReadConfig()
        {
            return _rootHub.RequestConfiguration();
        }

        public void WriteConfig(GlobalConfiguration config)
        {
            _rootHub.SubmitConfiguration(config);
        }

        /// <summary>
        ///     Receives details about the provided pad.
        /// </summary>
        /// <param name="pad">The pad ID to query details for.</param>
        /// <returns>The pad details returned from the root hub.</returns>
        public DualShockPadMeta Detail(DsPadId pad)
        {
            return _rootHub.GetPadDetail(pad);
        }

        /// <summary>
        ///     Submit a rumble request for a specified pad.
        /// </summary>
        /// <param name="pad">The target pad.</param>
        /// <param name="large">Rumble with the large (typically left) motor.</param>
        /// <param name="small">Rumble with the small (typically right) motor.</param>
        /// <returns>Returns request status.</returns>
        public bool Rumble(DsPadId pad, byte large, byte small)
        {
            return _rootHub.Rumble(pad, large, small);
        }

        public IEnumerable<IDualShockProfile> GetProfiles()
        {
            return _profileManager.GetProfiles();
        }

        public void SubmitProfile(IDualShockProfile profile)
        {
            _profileManager.SubmitProfile(profile);
        }

        public void RemoveProfile(IDualShockProfile profile)
        {
            _profileManager.RemoveProfile(profile);
        }

        #endregion

        #region Component actions

        public bool Start()
        {
            try
            {
                if (!IsActive)
                {
                    #region WCF client

                    var address = new EndpointAddress(new Uri("net.tcp://localhost:26760/ScpRootHubService"));
                    var binding = new NetTcpBinding
                    {
                        TransferMode = TransferMode.Streamed,
                        Security = new NetTcpSecurity {Mode = SecurityMode.None}
                    };
                    var factory = new ChannelFactory<IScpCommandService>(binding, address);

                    _rootHub = factory.CreateChannel(address);

                    #endregion

                    #region Feed client

                    var rootHubFeedChannel = new ScpNativeFeedChannel(_rxFeedClient);
                    rootHubFeedChannel.Receiver.SubscribeOn(TaskPoolScheduler.Default).Subscribe(buffer =>
                    {
                        if (buffer.Length <= 0)
                            return;

                        //TODO: deserialize OnFeedPacketReceived(ScpHidReportFactory.Create(buffer));
                    });

                    _rxFeedClient.ConnectAsync();

                    #endregion

                    IsActive = true;
                }
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("Unexpected error: {0}", ex);
            }

            return IsActive;
        }

        public bool Stop()
        {
            // TODO: refactor useless bits
            try
            {
                if (IsActive)
                {
                    IsActive = false;
                    //_rxFeedClient.Disconnect();
                }
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("Unexpected error: {0}", ex);
            }

            return !IsActive;
        }

        #endregion

        #region Ctors

        public ScpProxy()
        {
            InitializeComponent();
        }

        public ScpProxy(IContainer container)
            : this()
        {
            container.Add(this);
        }

        #endregion

        #region Public events

        public event EventHandler<IScpHidReport> NativeFeedReceived;

        public event EventHandler<EventArgs> RootHubDisconnected;

        #endregion

        #region Event methods

        private void OnFeedPacketReceived(IScpHidReport data)
        {
            _packetCache[data.PadId] = data;

            if (NativeFeedReceived != null)
            {
                NativeFeedReceived(this, data);
            }
        }

        private void OnRootHubDisconnected(object sender, EventArgs args)
        {
            if (RootHubDisconnected != null)
            {
                RootHubDisconnected(sender, args);
            }
        }

        #endregion
    }
}