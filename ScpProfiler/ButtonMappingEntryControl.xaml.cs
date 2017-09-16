using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using WindowsInput;
using WindowsInput.Native;
using ScpControl.Shared.Core;
using ScpControl.Utilities;
using Bindables;

namespace ScpProfiler
{
    /// <summary>
    ///     Interaction logic for ButtonMappingEntryControl.xaml
    /// </summary>
    public partial class ButtonMappingEntryControl : UserControl
    {
        #region Ctor

        public ButtonMappingEntryControl()
        {
            ButtonProfile = new DsButtonProfile();

            InitializeComponent();

            CurrentCommandTypeView = new CollectionView(AvailableCommandTypes);
            CurrentCommandTargetView = new CollectionView(AvailableKeys);

            CurrentCommandTypeView.MoveCurrentTo(AvailableCommandTypes.First());
            CurrentCommandTargetView.MoveCurrentTo(AvailableKeys.First());

            CurrentCommandTypeView.CurrentChanged += CurrentCommandTypeOnCurrentChanged;
            CurrentCommandTargetView.CurrentChanged += CurrentCommandTargetOnCurrentChanged;
        }

        #endregion

        private void CurrentCommandTargetOnCurrentChanged(object sender, EventArgs eventArgs)
        {
            ButtonProfile.MappingTarget = (IMappingTarget)CurrentCommandTargetView.CurrentItem;
        }

        private void CurrentCommandTypeOnCurrentChanged(object sender, EventArgs eventArgs)
        {
            ButtonProfile.MappingTarget=
                (IMappingTarget)
                    Enum.ToObject(typeof(IMappingTarget), ((EnumMetaData)CurrentCommandTypeView.CurrentItem).Value);

            if (ButtonProfile.MappingTarget is Keystrokes)
            {
                CurrentCommandTargetView = new CollectionView(AvailableKeys);

            }
            if (ButtonProfile.MappingTarget is MouseButtons)
            {
                CurrentCommandTargetView = new CollectionView(AvailableMouseButtons);
            }

            CurrentCommandTargetView.MoveCurrentToFirst();
            CurrentCommandTargetView.CurrentChanged += CurrentCommandTargetOnCurrentChanged;
        }

        /// <summary>
        ///     Tries to convert an object value to a <see cref="VirtualKeyCode" />.
        /// </summary>
        /// <param name="o">An object containing the <see cref="VirtualKeyCode" /> index.</param>
        /// <returns>The corresponding <see cref="VirtualKeyCode" />.</returns>
        private static VirtualKeyCode ToVirtualKeyCode(object o)
        {
            return o != null
                ? (VirtualKeyCode) Enum.Parse(typeof (VirtualKeyCode), o.ToString())
                : AvailableKeys.First();
        }

        #region Private control events

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            DependencyPropertyDescriptor
                .FromProperty(ButtonProfileProperty, typeof (ButtonMappingEntryControl))
                .AddValueChanged(this, (s, args) =>
                {
                    if (ButtonProfile == null) return;

                    CurrentCommandTypeView = new CollectionView(AvailableCommandTypes);
                    //CurrentCommandTypeView.MoveCurrentToPosition((int) ButtonProfile.MappingTarget);

                    if (ButtonProfile.MappingTarget is GamepadButton)
                    {
                        CurrentCommandTargetView.MoveCurrentTo(ButtonProfile.MappingTarget);
                    }
                    if (ButtonProfile.MappingTarget is Keystrokes)
                    {
                        CurrentCommandTargetView.MoveCurrentTo(
                            AvailableKeys.FirstOrDefault(
                                k => k == ToVirtualKeyCode(ButtonProfile.MappingTarget)));

                    }
                    if (ButtonProfile.MappingTarget is MouseButtons)
                    {
                        CurrentCommandTargetView = new CollectionView(AvailableMouseButtons);
                    }


                    CurrentCommandTypeView.CurrentChanged += CurrentCommandTypeOnCurrentChanged;
                    CurrentCommandTargetView.CurrentChanged += CurrentCommandTargetOnCurrentChanged;
                });
        }

        #endregion

        #region Private static fields

        private static readonly IList<EnumMetaData> AvailableCommandTypes =
            EnumExtensions.GetValuesAndDescriptions(typeof (IMappingTarget)).ToList();

        private static readonly IList<VirtualKeyCode> AvailableKeys = Enum.GetValues(typeof (VirtualKeyCode))
            .Cast<VirtualKeyCode>()
            .Where(k => k != VirtualKeyCode.MODECHANGE
                        && k != VirtualKeyCode.PACKET
                        && k != VirtualKeyCode.NONAME
                        && k != VirtualKeyCode.LBUTTON
                        && k != VirtualKeyCode.RBUTTON
                        && k != VirtualKeyCode.MBUTTON
                        && k != VirtualKeyCode.XBUTTON1
                        && k != VirtualKeyCode.XBUTTON2
                        && k != VirtualKeyCode.HANGEUL
                        && k != VirtualKeyCode.HANGUL).ToList();


        private static readonly IList<MouseButton> AvailableMouseButtons =
            Enum.GetValues(typeof (MouseButton)).Cast<MouseButton>().ToList();

        #endregion

        #region Dependency properties

        [DependencyProperty]
        public ImageSource IconSource { get; set; }

        [DependencyProperty]
        public string IconToolTip { get; set; }

        public DsButtonProfile ButtonProfile
        {
            get { return (DsButtonProfile) GetValue(ButtonProfileProperty); }
            set { SetValue(ButtonProfileProperty, value); }
        }

        public static readonly DependencyProperty ButtonProfileProperty =
            DependencyProperty.Register("ButtonProfile", typeof (DsButtonProfile),
                typeof (ButtonMappingEntryControl));

        [DependencyProperty]
        public ICollectionView CurrentCommandTypeView { get; set; }

        [DependencyProperty]
        public ICollectionView CurrentCommandTargetView { get; set; }

        #endregion
    }
}
