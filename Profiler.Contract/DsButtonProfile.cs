using System.Runtime.Serialization;
using HidReport.Contract.Enums;

namespace Profiler.Contract
{
    /// <summary>
    ///     Describes details about individual buttons.
    /// </summary>
    [DataContract]
    public class DsButtonProfile
    {
        #region Ctor
        /// <summary>
        ///     Creates a new button mapping profile.
        /// </summary>
        /// <param name="source">A DualShock button which will be affected by this profile.</param>
        /// <param name="target"></param>
        public DsButtonProfile(ButtonsEnum source, IMappingTarget target)
        {
            Turbo = new DsButtonProfileTurboSetting();
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
        #endregion
    }
}