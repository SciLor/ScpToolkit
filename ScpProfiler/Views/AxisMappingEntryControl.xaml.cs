using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using WindowsInput;
using WindowsInput.Native;
using Bindables;
using ScpControl.Shared.Core;
using ScpControl.Utilities;

namespace ScpProfiler
{
    /// <summary>
    ///     Interaction logic for ButtonMappingEntryControl.xaml
    /// </summary>
    public partial class AxisMappingEntryControl : UserControl
    {

        #region Ctor

        public AxisMappingEntryControl()
        {
            InitializeComponent();
        }

        #endregion

        #region Private event handlers

        private void TargetTypeComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void TargetCommandComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        #endregion
    }
}