using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.ServiceProcess;
using System.Threading.Tasks;
using Config;
using log4net;
using NativeBusControl;
using NativeLayer.Database;
using RootHub.Contract.Exceptions;

namespace ScpService
{
    public partial class ScpService : ServiceBase
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private RootHub.RootHub _rootHub;
        public ScpService()
        {
            InitializeComponent();

            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                Log.FatalFormat("An unhandled exception occured: {0}", args.ExceptionObject);
            };
        }

        protected override void OnStart(string[] args)
        {
            //TODO: we can report service SERVICE_START_PENDING status
            var sw = Stopwatch.StartNew();

            Log.Info("DSx Service Starting");

            Log.DebugFormat("++ {0} {1}", Assembly.GetExecutingAssembly().Location,
                    Assembly.GetExecutingAssembly().GetName().Version);

            Log.DebugFormat("Setting working directory to {0}", GlobalConfiguration.AppDirectory);
            Directory.SetCurrentDirectory(GlobalConfiguration.AppDirectory);

            Log.DebugFormat("Setting process priority to {0}", GlobalConfiguration.Instance.ServiceProcessPriority);
            Process.GetCurrentProcess().PriorityClass = GlobalConfiguration.Instance.ServiceProcessPriority;

            var installTask = Task.Run(() =>
            {
                try
                {
                    using (var db = new ScpDb())
                    {
                        //TODO: intstll drivers
                    }
                }
                catch (Exception ex)
                {
                    Log.FatalFormat("Error during driver installation: {0}", ex);
                    Stop();
                }
            });

            Log.DebugFormat("Time spent 'till Root Hub start: {0}", sw.Elapsed);

            try
            {
                _rootHub = new RootHub.RootHub();
            }
            catch (RootHubAlreadyStartedException rhex)
            {
                Log.Fatal($"Couldn't start the root hub: {rhex}");
                Stop();
            }

            Log.DebugFormat("Time spent 'till registering notifications: {0}", sw.Elapsed);
            Log.DebugFormat("Total Time spent in Service Start method: {0}", sw.Elapsed);
        }

        protected override void OnStop()
        {
            _rootHub.Dispose();
            _rootHub = null;
            Log.Info("DSx Service Stopped");
        }
    }
}
