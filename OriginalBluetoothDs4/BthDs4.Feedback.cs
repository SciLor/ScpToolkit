using System;
using System.Drawing;
using HidReport.Contract.Core;

namespace NativeLayer.Bluetooth.Ds4
{
    internal partial class BthDs4
    {
        private const int R = 9; // Led Offsets
        private const int G = 10; // Led Offsets
        private const int B = 11; // Led Offsets

        public virtual void SendFeedback(IScpFeedback feedback)
        {
            Rumble(feedback.RumbleBig, feedback.RumbleSmall);
            SetLightBarColor(feedback.LightBarColor);
        }

        private void SetLightBarColor(Color color)
        {
            _hidReport[R] = color.R;
            _hidReport[G] = color.G;
            _hidReport[B] = color.B;
            Queued = 1;
        }

        /// <summary>
        ///     Send Rumble request to controller.
        /// </summary>
        /// <param name="large">Larg motor.</param>
        /// <param name="small">Small motor.</param>
        /// <returns>Always true.</returns>
        private void Rumble(byte large, byte small)
        {
            lock (_hidReport)
            {
                _hidReport[7] = small;
                _hidReport[8] = large;

                if (!Blocked)
                {
                    Last = DateTime.Now;
                    Blocked = true;
                    BluetoothDongle.HID_Command(HciHandle.Bytes, Get_SCID(L2CAP.PSM.HID_Command), _hidReport);
                }
                else
                {
                    Queued = 1;
                }
            }
        }

        #endregion
    }
}