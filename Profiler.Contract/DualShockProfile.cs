using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;
using WindowsInput.Native;
using HidReport.Contract.Core;
using HidReport.Contract.Enums;
using Profiler.Contract.MappingTargets;
using ScpControl.Shared.Core;
using MouseButton = WindowsInput.MouseButton;

namespace Profiler.Contract
{
    /// <summary>
    ///     Represents a DualShock button/axis mapping profile.
    /// </summary>
    [DataContract]
    [KnownType(typeof(VirtualKeyCode))]
    [KnownType(typeof(MouseButton))]
    [DisplayName("DualShock Profile")]
    public class DualShockProfile
    {
        #region Ctor

        public static DualShockProfile DefaultProfile()
        {

            return new DualShockProfile()
            {
                IsActive = true,
                Match = DsMatch.Global,
                Id = Guid.NewGuid(),
                Name = "Default profile",
                Buttons = new List<DsButtonProfile>
                {
                    new DsButtonProfile(ButtonsEnum.Ps      , new GamepadButton(X360Button.Guide)),
                    new DsButtonProfile(ButtonsEnum.Circle  , new GamepadButton(X360Button.B)),
                    new DsButtonProfile(ButtonsEnum.Cross   , new GamepadButton(X360Button.A)),
                    new DsButtonProfile(ButtonsEnum.Square  , new GamepadButton(X360Button.X)),
                    new DsButtonProfile(ButtonsEnum.Triangle, new GamepadButton(X360Button.Y)),
                    new DsButtonProfile(ButtonsEnum.L1      , new GamepadButton(X360Button.LB)),
                    new DsButtonProfile(ButtonsEnum.R1      , new GamepadButton(X360Button.RB)),
                    //new DsButtonProfile(ButtonsEnum.L2      , new GamepadButton(X360Button.Start)),
                    //new DsButtonProfile(ButtonsEnum.R2      , new GamepadButton(X360Button.Start)),
                    new DsButtonProfile(ButtonsEnum.L3      , new GamepadButton(X360Button.LS)),
                    new DsButtonProfile(ButtonsEnum.R3      , new GamepadButton(X360Button.RS)),
                    new DsButtonProfile(ButtonsEnum.Up      , new GamepadButton(X360Button.Up)),
                    new DsButtonProfile(ButtonsEnum.Right   , new GamepadButton(X360Button.Right)),
                    new DsButtonProfile(ButtonsEnum.Down    , new GamepadButton(X360Button.Down)),
                    new DsButtonProfile(ButtonsEnum.Left    , new GamepadButton(X360Button.Left)),
                    new DsButtonProfile(ButtonsEnum.Share   , new GamepadButton(X360Button.Back)),
                    new DsButtonProfile(ButtonsEnum.Options , new GamepadButton(X360Button.Start)),
                    new DsButtonProfile(ButtonsEnum.Select , new GamepadButton(X360Button.Back)),
                    new DsButtonProfile(ButtonsEnum.Start , new GamepadButton(X360Button.Start)),
                }
            };
        }

        public DualShockProfile()
        {
            Id = Guid.NewGuid();
            Name = "New Profile";
        }

        [OnDeserializing]
        private void OnDeserializing(StreamingContext c)
        {
        }

        [DataMember]
        [Browsable(false)]
        public List<DsButtonProfile> Buttons { get; set; }

        #endregion

        #region Public methods

        public override bool Equals(object obj)
        {
            var profile = obj as DualShockProfile;

            return profile != null && profile.Name.Equals(Name);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode() ^ Name.GetHashCode();
        }

        public override string ToString()
        {
            return Name;
        }

        #endregion

        #region Properties

        [DataMember]
        [Category("Main")]
        [DisplayName("Profile is active")]
        [Description("If disabled, the entire profile will be ignored from processing.")]
        public bool IsActive { get; set; }

        [DataMember]
        [Category("Main")]
        [DisplayName("Profile name")]
        [Description("The friendly name of this profile.")]
        public string Name { get; set; }

        [DataMember]
        [ReadOnly(true)]
        [Description("The unique identifier of this profile.")]
        public Guid Id { get; private set; }

        [DataMember]
        [ReadOnly(true)]
        [DisplayName("Pad ID")]
        public DsPadId PadId { get; set; }

        [DataMember]
        [ReadOnly(true)]
        [DisplayName("MAC Address")]
        public string MacAddress { get; set; }

        [DataMember]
        [ReadOnly(true)]
        [DisplayName("Pad Model")]
        public DsModel Model { get; set; }

        [DataMember]
        [Category("Main")]
        [DisplayName("Match profile on")]
        public DsMatch Match { get; set; }
        #endregion
    }

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
        ///     Applies turbo algorithm for a specified <see cref="IDsButton" /> on a given <see cref="ScpHidReport" />.
        /// </summary>
        /// <param name="report">The HID report to manipulate.</param>
        /// <param name="button">The button to trigger turbo on.</param>
        public bool ApplyOn(IScpHidReport report, ButtonsEnum button)
        {
            // if button got released...
            if (_isActive && !report[button].IsPressed)
            {
                // ...disable, reset and return
                _isActive = false;
                _delayedFrame.Reset();
                _engagedFrame.Reset();
                _releasedFrame.Reset();
                return false;
            }

            // if turbo is enabled and button is pressed...
            //TODO: seems a bug (!_isActive)
            if (!_isActive && report[button].IsPressed)
            {
                // ...start calculating the activation delay...
                if (!_delayedFrame.IsRunning) _delayedFrame.Restart();

                // ...if we are still activating, don't do anything
                if (_delayedFrame.ElapsedMilliseconds < Delay) return true;

                // time to activate!
                _isActive = true;
                _delayedFrame.Reset();
            }

            // if the button was released...
            if (!report[button].IsPressed)
            {
                // ...restore default states and skip processing
                _isActive = false;
                return false;
            }

            // reset engaged ("keep pressed") time frame...
            if (!_engagedFrame.IsRunning) _engagedFrame.Restart();

            // ...do not change state while within frame and button is still pressed, then skip
            if (_engagedFrame.ElapsedMilliseconds < Interval && report[button].IsPressed) return true;

            // reset released time frame ("forecefully release") for button
            if (!_releasedFrame.IsRunning) _releasedFrame.Restart();

            // while we're still within the released time frame...
            if (_releasedFrame.ElapsedMilliseconds < Release)
            {
                // ...re-set the button state to released
                return false;
            }
            else
            {
                // all frames passed, reset and start over
                _isActive = false;

                _delayedFrame.Stop();
                _engagedFrame.Stop();
                _releasedFrame.Stop();
                return true;
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