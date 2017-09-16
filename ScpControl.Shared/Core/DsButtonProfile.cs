using System.Runtime.Serialization;
using WindowsInput;
using HidReport.Contract.Enums;

namespace ScpControl.Shared.Core
{
    /// <summary>
    ///     Describes details about individual buttons.
    /// </summary>
    [DataContract]
    public class DsButtonProfile
    {
        private const uint InputDelay = 100;

        #region Ctor

        public DsButtonProfile()
        {
            OnCreated();
        }

        /// <summary>
        ///     Creates a new button mapping profile.
        /// </summary>
        /// <param name="source">A DualShock button which will be affected by this profile.</param>
        public DsButtonProfile(ButtonsEnum source, IMappingTarget target) : this()
        {
            SourceButton = source;
            MappingTarget = target;
        }

        #endregion

        #region Properties

        [DataMember]
        public ButtonsEnum SourceButton { get; set; }

        [DataMember]
        public IMappingTarget MappingTarget { get; set; }

        [DataMember]
        public DsButtonProfileTurboSetting Turbo { get; set; }

        public byte CurrentValue { get; set; }

        #endregion

        #region Deserialization

        private void OnCreated()
        {
            //MappingTarget = new DsButtonMappingTarget();
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