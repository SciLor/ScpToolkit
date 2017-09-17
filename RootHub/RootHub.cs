using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using Config;
using HidReport.Contract.Core;
using log4net;
using Libarius.System;
using Mapper;
using Mapper.Contract;
using NativeLayer.Contract;
using NativeLayer.ScpBus;
using RootHub.Contract;
using RootHub.Contract.Exceptions;
using Utilites;

namespace RootHub
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = false, InstanceContextMode = InstanceContextMode.Single)]
    public sealed class RootHub : IScpCommandService, IDisposable
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly List<IDeviceHub> _hubs = new List<IDeviceHub>()
        {
            //TODO: new BthHub(),
            new UsbHub()
        };

        /// <summary>
        ///     A collection of currently connected game pads.
        /// </summary>
        private readonly List<IGamepad> _pads = new List<IGamepad>();

        private IMapper _mapper;

        // creates a system-wide mutex to check if the root hub has been instantiated already
        private readonly LimitInstance _limitInstance;

        #region IScpCommandService methods

        public bool Rumble(Guid guid, byte large, byte small)
        {
            //TODO: if device can rumble
            //TODO: select pad
            IGamepad device = _pads.First();
            if (device == null)
                return false;
            //TODO: create feedback
            IScpFeedback feedback = null;
            device.SendFeedback(feedback);
            return true;
        }

        public IEnumerable<string> GetStatusData()
        {
            return _pads.Select(p=>p.ToString());
        }

        /// <summary>
        ///     Requests the currently active configuration set from the root hub.
        /// </summary>
        /// <returns>Returns the global configuration object.</returns>
        public GlobalConfiguration RequestConfiguration()
        {
            return GlobalConfiguration.Request();
        }

        /// <summary>
        ///     Submits an altered copy of the global configuration to the root hub and saves it.
        /// </summary>
        /// <param name="configuration">The global configuration object.</param>
        public void SubmitConfiguration(GlobalConfiguration configuration)
        {
            GlobalConfiguration.Submit(configuration);
            GlobalConfiguration.Save();
        }

        #endregion

        ///     Opens and initializes devices and services listening and running on the local machine.
        public RootHub()
        {
            Log.Debug("Initializing root hub");
            Log.DebugFormat("++ {0} {1}", Assembly.GetExecutingAssembly().Location,
                Assembly.GetExecutingAssembly().GetName().Version);
            Log.DebugFormat("++ {0}", OsInfoHelper.OsInfo);

            _limitInstance = new LimitInstance(@"Global\ScpDsxRootHub");

            try
            {
                if (!_limitInstance.IsOnlyInstance) // existing root hub running as desktop app
                    throw new RootHubAlreadyStartedException(
                        "The root hub is already running, please close the ScpServer first!");
            }
            catch (UnauthorizedAccessException) // existing root hub running as service
            {
                throw new RootHubAlreadyStartedException(
                    "The root hub is already running, please stop the ScpService first!");
            }

            // subscribe to device plug-in events
            foreach (var bus in _hubs)
            {
                bus.Arrival += OnDeviceArrival;
                bus.Report += OnHidReportReceived;
            }


            GlobalConfiguration.Load();
        }

        #region Actions

        /// <summary>
        ///     Stops all underlying hubs, disposes acquired resources and saves the global configuration.
        /// </summary>
        /// <returns>True on success, false otherwise.</returns>
        public void Dispose()
        {
            Log.Debug("Root hub stop requested");

            _pads.Clear();
            foreach (var bus in _hubs)
                bus.Dispose();

            Log.Debug("Root hub stopped");

            _limitInstance.Dispose();

            GlobalConfiguration.Save();
        }
        #endregion

        private void OnDeviceArrival(object sender, ArrivalEventArgs e)
        {
            var arrived = e.Device;

            lock (_pads)
            {
                //TODO: find PAD by GUID
                //TODO: remove old one if exists
                //TODO: add new one to arrays
                //TODO: notify Mapper
            }

            //TODO what is VBus if (GlobalConfiguration.Instance.IsVBusDisabled) return true;
        }

        private void OnHidReportReceived(object sender, IScpHidReport e)
        {
            _mapper.OnHidreportReceived(sender, e);
            //if (GlobalConfiguration.Instance.AlwaysUnPlugVirtualBusDevice)
            //{
            //    Unplug(dev);
            //}
        }
    }
}