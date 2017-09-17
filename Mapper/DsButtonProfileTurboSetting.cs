using System.Diagnostics;
using System.Runtime.Serialization;
using HidReport.Contract.Core;
using HidReport.Contract.Enums;

namespace Mapper
{
    /// <summary>
    ///     Describes button turbo mode details.
    /// </summary>
    [DataContract]
    public class DsButtonProfileTurboSetting
    {
        #region Ctor

        public DsButtonProfileTurboSetting()
        {
            Delay = 0;
            Interval = 50;
            Release = 100;
        }

        #endregion

        #region Public methods

        /// <summary>
        ///     Applies turbo algorithm for a specified <see cref="IDsButton" /> on a given <see cref="IScpHidReport" />.
        /// </summary>
        /// <param name="report">The HID report to manipulate.</param>
        /// <param name="button">The button to trigger turbo on.</param>
        public void ApplyOn(IScpHidReport report, ButtonsEnum button)
        {
            // if button got released...
            if (_isActive && !report[button].IsPressed)
            {
                // ...disable, reset and return
                _isActive = false;
                _delayedFrame.Reset();
                _engagedFrame.Reset();
                _releasedFrame.Reset();
                return;
            }

            // if turbo is enabled and button is pressed...
            if (!_isActive && report[button].IsPressed)
            {
                // ...start calculating the activation delay...
                if (!_delayedFrame.IsRunning) _delayedFrame.Restart();

                // ...if we are still activating, don't do anything
                if (_delayedFrame.ElapsedMilliseconds < Delay) return;

                // time to activate!
                _isActive = true;
                _delayedFrame.Reset();
            }

            // if the button was released...
            if (!report[button].IsPressed)
            {
                // ...restore default states and skip processing
                _isActive = false;
                return;
            }

            // reset engaged ("keep pressed") time frame...
            if (!_engagedFrame.IsRunning) _engagedFrame.Restart();

            // ...do not change state while within frame and button is still pressed, then skip
            if (_engagedFrame.ElapsedMilliseconds < Interval && report[button].IsPressed) return;

            // reset released time frame ("forecefully release") for button
            if (!_releasedFrame.IsRunning) _releasedFrame.Restart();

            // while we're still within the released time frame...
            if (_releasedFrame.ElapsedMilliseconds < Release)
            {
                // ...re-set the button state to released
                //TODO: report.Unset(button);
            }
            else
            {
                // all frames passed, reset and start over
                _isActive = false;

                _delayedFrame.Stop();
                _engagedFrame.Stop();
                _releasedFrame.Stop();
            }
        }

        #endregion

        #region Private fields

        private Stopwatch _delayedFrame = new Stopwatch();
        private Stopwatch _engagedFrame = new Stopwatch();
        private bool _isActive;
        private Stopwatch _releasedFrame = new Stopwatch();

        #endregion

        #region Properties

        /// <summary>
        ///     True if turbo mode is enabled for the current button, false otherwise.
        /// </summary>
        [DataMember]
        public bool IsEnabled { get; set; }

        /// <summary>
        ///     The delay (in milliseconds) afther which the turbo mode shall engage (default is immediate).
        /// </summary>
        [DataMember]
        public int Delay { get; set; }

        /// <summary>
        ///     The timespan (in milliseconds) the button should be reported as remaining pressed to the output.
        /// </summary>
        [DataMember]
        public int Interval { get; set; }

        /// <summary>
        ///     The timespan (in milliseconds) the button state should be reported as released so the turbo event can repeat again.
        /// </summary>
        [DataMember]
        public int Release { get; set; }

        #endregion

        #region Deserialization

        private void OnCreated()
        {
            _delayedFrame = new Stopwatch();
            _engagedFrame = new Stopwatch();
            _releasedFrame = new Stopwatch();
            _isActive = false;
        }

        [OnDeserializing]
        private void OnDeserializing(StreamingContext c)
        {
            OnCreated();
        }

        #endregion
    }
}