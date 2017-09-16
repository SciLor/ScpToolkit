using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using HidReport.Contract.Enums;
using ScpControl;
using ScpControl.Shared.Core;
using Xceed.Wpf.Toolkit;
using MessageBox = Xceed.Wpf.Toolkit.MessageBox;


namespace ScpProfiler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ProfileManagerViewModel _vm = new ProfileManagerViewModel();

        public MainWindow()
        {
            InitializeComponent();

            ProfilesCollectionControl.NewItemTypes = new List<Type>() {typeof (DualShockProfile)};
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            MainGrid.DataContext = _vm;
        }


        private void CurrentPad_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            //_proxy.SubmitProfile(_vm.CurrentProfile);
        }

        private void EditButton_OnClick(object sender, RoutedEventArgs e)
        {
            EditProfileChildWindow.Show();
        }

        private void ProfilesCollectionControl_OnItemAdded(object sender, ItemEventArgs e)
        {
            //_proxy.SubmitProfile(e.Item as DualShockProfile);
        }

        private void ProfilesCollectionControl_OnItemDeleted(object sender, ItemEventArgs e)
        {
            //_proxy.RemoveProfile(e.Item as DualShockProfile);
        }
    }
}
