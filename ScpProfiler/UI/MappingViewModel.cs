using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using HidReport.Contract.Enums;
using Profiler.Contract;
using Profiler.Contract.MappingTargets;
using ScpControl.Shared.Core;
using ScpProfiler.UI.MappingSources;
using ScpProfiler.UI.MappingTargets;

namespace ScpProfiler.UI
{
    internal class MappingViewModel : INotifyPropertyChanged
    {
        private readonly DsButtonProfile _buttonProfile;

        public MappingViewModel():this(new DsButtonProfile(ButtonsEnum.Circle, new GamepadButton(X360Button.B)))
        {
            
        }

        public MappingViewModel(DsButtonProfile buttonProfile)
        {
            _buttonProfile = buttonProfile;
            //TODO: pass notifier
            MappingSourceViewModel = new MappingSourceViewModel(buttonProfile.SourceButton, null);
            MappingTargetViewModel = new MappingTargetViewModel(buttonProfile.MappingTarget);
            MappingSources = new ObservableCollection<MappingSourceViewModel>()
            {
                MappingSourceViewModel
            };
            MappingTargets = new ObservableCollection<MappingTargetViewModel>()
            {
                MappingTargetViewModel
            };
        }

        public MappingSourceViewModel MappingSourceViewModel { get; }
        public MappingTargetViewModel MappingTargetViewModel { get; }
        public ObservableCollection<MappingSourceViewModel> MappingSources { get; }
        public ObservableCollection<MappingTargetViewModel> MappingTargets { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    //    private static readonly IList<EnumMetaData> AvailableCommandTypes =
    //EnumExtensions.GetValuesAndDescriptions(typeof(IMappingTarget)).ToList();

    //    private static readonly IList<VirtualKeyCode> AvailableKeys = Enum.GetValues(typeof(VirtualKeyCode))
    //        .Cast<VirtualKeyCode>()
    //        .Where(k => k != VirtualKeyCode.MODECHANGE
    //                    && k != VirtualKeyCode.PACKET
    //                    && k != VirtualKeyCode.NONAME
    //                    && k != VirtualKeyCode.LBUTTON
    //                    && k != VirtualKeyCode.RBUTTON
    //                    && k != VirtualKeyCode.MBUTTON
    //                    && k != VirtualKeyCode.XBUTTON1
    //                    && k != VirtualKeyCode.XBUTTON2
    //                    && k != VirtualKeyCode.HANGEUL
    //                    && k != VirtualKeyCode.HANGUL).ToList();


    //    private static readonly IList<MouseButton> AvailableMouseButtons =
    //        Enum.GetValues(typeof(MouseButton)).Cast<MouseButton>().ToList();


    //    [DependencyProperty]
    //    public ImageSource IconSource { get; set; }

    //    [DependencyProperty]
    //    public string IconToolTip { get; set; }

    //    public DsButtonProfile ButtonProfile
    //    {
    //        get { return (DsButtonProfile)GetValue(ButtonProfileProperty); }
    //        set { SetValue(ButtonProfileProperty, value); }
    //    }

    //    public static readonly DependencyProperty ButtonProfileProperty =
    //        DependencyProperty.Register("ButtonProfile", typeof(DsButtonProfile),
    //            typeof(MappingView));

    //    [DependencyProperty]
    //    public ICollectionView CurrentCommandTypeView { get; set; }

    //    [DependencyProperty]
    //    public ICollectionView CurrentCommandTargetView { get; set; }



    }
}