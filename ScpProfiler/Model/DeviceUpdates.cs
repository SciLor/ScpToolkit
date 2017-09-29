using System;
using HidReport.Contract.Core;

namespace ScpProfiler
{
    interface IHidReportNotifier
    {
        event EventHandler<IScpHidReport> OnHidReportReceived;
    }
}
