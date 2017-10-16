using System;
using HidReport.Contract.Core;

namespace ScpProfiler.Model
{
    interface IHidReportNotifier
    {
        event EventHandler<IScpHidReport> OnHidReportReceived;
    }
}
