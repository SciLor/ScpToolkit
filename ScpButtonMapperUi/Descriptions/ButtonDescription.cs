namespace ScpProfiler.Descriptions
{
    #region Interfaces

    #endregion

    /// <summary>
    ///     Implements a DualShock button.
    /// </summary>
    public sealed class ButtonDescription
    {
        #region Ctors

        public ButtonDescription(string name)
        {
            Name = name;
            DisplayName = name;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     The short name identifying the button.
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     A short descriptive name of the button.
        /// </summary>
        public string DisplayName { get; set; }
        #endregion

        #region Methods

        public override string ToString()
        {
            return DisplayName;
        }

        #endregion
    }
}