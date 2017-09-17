using System;
using System.Reflection;
using HidReport.Contract.Core;
using log4net;

namespace NativeLayer.Contract
{
    public abstract class DeviceHubBase : IDeviceHub
    {
        public event EventHandler<ArrivalEventArgs> Arrival;
        public event EventHandler<IScpHidReport> Report;

        public abstract void Dispose();

        protected static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        protected virtual void LogArrival(IGamepad arrived)
        {
            var args = new ArrivalEventArgs(arrived);
            Arrival?.Invoke(this, args);
        }

        protected virtual void OnHidReportReceived(object sender, IScpHidReport e)
        {
            Report?.Invoke(sender, e);
        }

    }
}
