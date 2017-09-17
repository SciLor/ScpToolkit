using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using WindowsInput;
using WindowsInput.Native;
using HidReport.Contract.Core;
using HidReport.Contract.Enums;
using Mapper.Contract;

namespace Mapper
{
    /// <summary>
    ///     Possible mapping target types (keystrokes, mouse movement etc.)
    /// </summary>
    public enum CommandType : byte
    {
        [Description("Keystrokes")] Keystrokes,
        [Description("Gamepad buttons")] GamepadButton,
        [Description("Mouse buttons")] MouseButtons,
        [Description("Mouse axis")] MouseAxis
    }

    /// <summary>
    ///     Describes a mapping target.
    /// </summary>
    [DataContract]
    [KnownType(typeof(DsButtonMappingTarget))]
    public class DsButtonMappingTarget
    {
        #region Properties

        [DataMember]
        public CommandType CommandType { get; set; }

        [DataMember]
        public ButtonsEnum CommandTarget { get; set; }

        #endregion
    }

    /// <summary>
    ///     Represents a DualShock button/axis mapping profile.
    /// </summary>
    [DataContract]
    [KnownType(typeof(VirtualKeyCode))]
    [KnownType(typeof(MouseButton))]
    [DisplayName("DualShock Profile")]
    public class DualShockProfile : IDualShockProfile
    {
        #region Ctor

        public DualShockProfile()
        {
            Id = Guid.NewGuid();
            Name = "New Profile";

            OnCreated();
        }

        [OnDeserializing]
        private void OnDeserializing(StreamingContext c)
        {
            OnCreated();
        }

        /// <summary>
        ///     Initialize buttons/axes.
        /// </summary>
        private void OnCreated()
        {
            Ps = new DsButtonProfile(ButtonsEnum.Ps);
            Circle = new DsButtonProfile(ButtonsEnum.Circle);
            Cross = new DsButtonProfile(ButtonsEnum.Cross);
            Square = new DsButtonProfile(ButtonsEnum.Square);
            Triangle = new DsButtonProfile(ButtonsEnum.Triangle);
            Select = new DsButtonProfile(ButtonsEnum.Select);
            Start = new DsButtonProfile(ButtonsEnum.Start);
            LeftShoulder = new DsButtonProfile(ButtonsEnum.L1);
            RightShoulder = new DsButtonProfile(ButtonsEnum.R1);
            LeftTrigger = new DsButtonProfile(ButtonsEnum.L2);
            RightTrigger = new DsButtonProfile(ButtonsEnum.R2);
            LeftThumb = new DsButtonProfile(ButtonsEnum.L3);
            RightThumb = new DsButtonProfile(ButtonsEnum.R3);
            Up = new DsButtonProfile(ButtonsEnum.Up);
            Right = new DsButtonProfile(ButtonsEnum.Right);
            Down = new DsButtonProfile(ButtonsEnum.Down);
            Left = new DsButtonProfile(ButtonsEnum.Left);
        }

        #endregion

        #region Public methods

        /// <summary>
        ///     Applies button re-mapping to the supplied report.
        /// </summary>
        /// <param name="report">The report to manipulate.</param>
        public void Remap(IScpHidReport report)
        {
            // determine if profile should be applied
            switch (Match)
            {
                case DsMatch.Global:
                    // always apply
                    break;
                case DsMatch.Mac:
                    // applies of MAC address matches
                    var reportMac = report.PadMacAddress.ToString();
                    if (string.CompareOrdinal(MacAddress.Replace(":", string.Empty), reportMac) != 0) return;
                    break;
                case DsMatch.None:
                    // never apply
                    return;
                case DsMatch.Pad:
                    // applies if pad IDs match
                    if (PadId != report.PadId) return;
                    break;
            }

            // walk through all buttons
            foreach (var buttonProfile in Buttons)
            {
                buttonProfile.Remap(report);
            }
        }

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

        [Browsable(false)]
        private IEnumerable<DsButtonProfile> Buttons
        {
            get
            {
                var props = GetType().GetProperties().Where(pi => pi.PropertyType == typeof(DsButtonProfile));

                return props.Select(b => b.GetValue(this)).Cast<DsButtonProfile>();
            }
        }

        [DataMember]
        [Browsable(false)]
        public DsButtonProfile Ps { get; set; }

        [DataMember]
        [Browsable(false)]
        public DsButtonProfile Circle { get; set; }

        [DataMember]
        [Browsable(false)]
        public DsButtonProfile Cross { get; set; }

        [DataMember]
        [Browsable(false)]
        public DsButtonProfile Square { get; set; }

        [DataMember]
        [Browsable(false)]
        public DsButtonProfile Triangle { get; set; }

        [DataMember]
        [Browsable(false)]
        public DsButtonProfile Select { get; set; }

        [DataMember]
        [Browsable(false)]
        public DsButtonProfile Start { get; set; }

        [DataMember]
        [Browsable(false)]
        public DsButtonProfile LeftShoulder { get; set; }

        [DataMember]
        [Browsable(false)]
        public DsButtonProfile RightShoulder { get; set; }

        [DataMember]
        [Browsable(false)]
        public DsButtonProfile LeftTrigger { get; set; }

        [DataMember]
        [Browsable(false)]
        public DsButtonProfile RightTrigger { get; set; }

        [DataMember]
        [Browsable(false)]
        public DsButtonProfile LeftThumb { get; set; }

        [DataMember]
        [Browsable(false)]
        public DsButtonProfile RightThumb { get; set; }

        // D-Pad
        [DataMember]
        [Browsable(false)]
        public DsButtonProfile Up { get; set; }

        [DataMember]
        [Browsable(false)]
        public DsButtonProfile Right { get; set; }

        [DataMember]
        [Browsable(false)]
        public DsButtonProfile Down { get; set; }

        [DataMember]
        [Browsable(false)]
        public DsButtonProfile Left { get; set; }

        #endregion
    }
}