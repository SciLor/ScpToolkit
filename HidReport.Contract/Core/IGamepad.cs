using System;
using HidReport.Contract.Core;

namespace NativeLayer.Contract
{
    public interface IGamepad
    {
        event EventHandler<IScpHidReport> HidReportEventHandler;
        void SendFeedback(IScpFeedback feedback);
        //TODO: create method to report available feedback resources
        string Model { get; }
    }
}