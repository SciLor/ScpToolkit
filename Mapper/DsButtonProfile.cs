using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using WindowsInput;
using WindowsInput.Native;
using HidReport.Contract.Core;
using HidReport.Contract.Enums;

namespace Mapper
{
    /// <summary>
    ///     Describes details about individual buttons.
    /// </summary>
    [DataContract]
    public class DsButtonProfile
    {
        private static readonly InputSimulator VirtualInput = new InputSimulator();
        private const uint InputDelay = 100;

        #region Ctor

        public DsButtonProfile()
        {
            OnCreated();
        }

        /// <summary>
        ///     Creates a new button mapping profile.
        /// </summary>
        /// <param name="sources">A list of DualShock buttons which will be affected by this profile.</param>
        public DsButtonProfile(params ButtonsEnum[] sources) : this()
        {
            SourceButtons = sources;
        }

        #endregion

        #region Public methods

        /// <summary>
        ///     Applies button re-mapping to the supplied report.
        /// </summary>
        /// <param name="report">The report to manipulate.</param>
        public void Remap(IScpHidReport report)
        {
            // skip disabled mapping
            if (!IsEnabled) return;

            switch (MappingTarget.CommandType)
            {
                case CommandType.GamepadButton:
                    foreach (var button in SourceButtons)
                    {
                        // turbo is special, apply first
                        if (Turbo.IsEnabled)
                        {
                            Turbo.ApplyOn(report, button);
                        }

                        // get target button
                        ButtonsEnum target = MappingTarget.CommandTarget;

                        X360Button? res= DefaultMapping.Map(target);
                        //TODO:
                    }
                    break;
                case CommandType.Keystrokes:
                    foreach (var button in SourceButtons)
                    {
                        var target = (VirtualKeyCode) Enum.ToObject(typeof(VirtualKeyCode), MappingTarget.CommandTarget);

                        if (report[button].IsPressed)
                        {
                            VirtualInput.Keyboard.KeyDown(target);
                        }
                        else
                        {
                            VirtualInput.Keyboard.KeyUp(target);
                        }
                    }
                    break;
            }
        }

        #endregion

        #region Properties

        [DataMember]
        private IEnumerable<ButtonsEnum> SourceButtons { get; set; }

        [DataMember]
        public DsButtonMappingTarget MappingTarget { get; private set; }

        [DataMember]
        public bool IsEnabled { get; set; }

        [DataMember]
        public DsButtonProfileTurboSetting Turbo { get; set; }

        public byte CurrentValue { get; set; }

        #endregion

        #region Deserialization

        private void OnCreated()
        {
            MappingTarget = new DsButtonMappingTarget();
            Turbo = new DsButtonProfileTurboSetting();
        }

        [OnDeserializing]
        private void OnDeserializing(StreamingContext c)
        {
            OnCreated();
        }

        #endregion
    }
}