using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HidReport.Contract.Enums;
using HidReport.Core;

namespace WinUsbDeviceComman
{
    class GamePadBase
    {
        /// <summary>
        ///     Sends periodic status updates to the controller (HID Output reports).
        /// </summary>
        /// <param name="now">The current timestamp.</param>
        //protected override void Process(DateTime now)
        //{
        //    if (!Monitor.TryEnter(UsbDs3CommunicationHelper._hidReport))
        //        return;

        //    try
        //    {
        //        #region Quick Disconnect handling

        //        if (IsShutdown)
        //        {
        //            if ((now - Disconnect).TotalMilliseconds >= 2000)
        //            {
        //                Log.InfoFormat("Pad {0} disconnected due to quick disconnect combo", PadId);
        //                Stop();
        //                return;
        //            }
        //        }

        //        #endregion


        //        #region send HID Output Report
        //        ProcessSpecifigc();
        //        #endregion
        //    }
        //    finally
        //    {
        //        Monitor.Exit(UsbDs3CommunicationHelper._hidReport);
        //    }
        //}
        //TODO: implement fake controller
        //private void ProcessSpecifigc()
        //{
        //    var outReport = ReportDescriptor.OutputReports.FirstOrDefault();
        //    if (outReport == null)
        //        return;

        //    var buffer = new byte[outReport.Length + 1];
        //    System.Buffer.BlockCopy(_hidReport, 0, buffer, 1, _hidReport.Length);
        //    buffer[0] = outReport.ID;

        //    //Todo: WriteIntPipe(buffer, buffer.Length, ref transfered);
        //}

        public static bool IsQuickDisconnectPressed(ScpHidReport inputReport)
        {
            // detect Quick Disconnect combo (L1, R1 and PS buttons pressed at the same time)
            if (inputReport[ButtonsEnum.L1].IsPressed
                && inputReport[ButtonsEnum.R1].IsPressed
                && inputReport[ButtonsEnum.Ps].IsPressed)
            {
                // unset PS button
                inputReport.Unset(ButtonsEnum.Ps);
                return true;
            }
            return false;
        }
    }
}
