using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HidReport.Contract.Core;
using HidReport.Contract.Enums;

namespace ScpProfiler.Test
{
    internal class StubHidReportNotifier : IHidReportNotifier, IDisposable
    {
        private readonly CancellationTokenSource _cts;
        private readonly Task _t;

        public StubHidReportNotifier()
        {
            _cts = new CancellationTokenSource();
            //var hidReport = new HidReport.Core.HidReport()
            //{
            //    BatteryStatus = DsBattery.Medium,
            //};
            //_t = Run(() => { OnHidReportReceived?.Invoke(null, hidReport); },
            //    TimeSpan.FromMilliseconds(500)
            //    , _cts.Token
            //);
        }

        public static async Task Run(Action action, TimeSpan period, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(period, cancellationToken);

                if (!cancellationToken.IsCancellationRequested)
                    action();
            }
        }

        public event EventHandler<IScpHidReport> OnHidReportReceived;

        public void Dispose()
        {
            _cts.Cancel();
            _cts?.Dispose();
            _t?.Dispose();
        }
    }
}