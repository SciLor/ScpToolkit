namespace ScpProfiler.Descriptions
{
    /// <summary>
    ///     Implementes a DualShock axis.
    /// </summary>
    internal class AxisDescription
    {
        #region Ctors

        public AxisDescription(string name)
        {
            Name = name;
            DisplayName = name;
            DefaultValue = 0x00;
        }

        #endregion
        /// <summary>
        ///     The short name of the axis.
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     The descriptive name of the axis.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        ///     The default value of the axis reported at non-engaged state.
        /// </summary>
        public byte DefaultValue { get; set; }
    }
}
