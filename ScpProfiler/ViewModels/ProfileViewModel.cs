using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ScpControl.Shared.Core;
using ScpProfiler.Annotations;

namespace ScpProfiler
{
    internal class ProfileViewModel : INotifyPropertyChanged
    {
        private DualShockProfile _model;

        public ProfileViewModel(DualShockProfile model)
        {
            _model = model;
        }

        public ObservableCollection<SourceButtonViewModel> AvailableButtons { get; set; }

        public ObservableCollection<ButtonProfileViewModel> Buttons { get; set; }
        public ObservableCollection<AxisProfileViewModel> Axes { get; set; }
        public ObservableCollection<TouchpadProfileViewModel> Touchpads { get; set; }

        public ObservableCollection<SourceButtonViewModel> AvailableSourceButtons = null;
        public ObservableCollection<TargetButtonViewModel> AvailableTargetButtons = null;
        //public ObservableCollection<TargetKeyViewModel> AvailableSourceButtons = null;
        //public ObservableCollection<TargetMouseButtonViewModel> AvailableMouseButtons = null;


//        private static readonly IList<VirtualKeyCode> AvailableKeys = Enum.GetValues(typeof(VirtualKeyCode))
//.Cast<VirtualKeyCode>()
//.Where(k => k != VirtualKeyCode.MODECHANGE
//            && k != VirtualKeyCode.PACKET
//            && k != VirtualKeyCode.NONAME
//            && k != VirtualKeyCode.LBUTTON
//            && k != VirtualKeyCode.RBUTTON
//            && k != VirtualKeyCode.MBUTTON
//            && k != VirtualKeyCode.XBUTTON1
//            && k != VirtualKeyCode.XBUTTON2
//            && k != VirtualKeyCode.HANGEUL
//            && k != VirtualKeyCode.HANGUL).ToList();
        public event PropertyChangedEventHandler PropertyChanged;


        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
