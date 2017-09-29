using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ScpControl.Shared.Core;
using ScpProfiler.Annotations;

namespace ScpProfiler
{
    internal class ProfileViewModel : INotifyPropertyChanged
    {
        private readonly DualShockProfile _model;
        public string Name { get; set; }
        public string Model { get; set; }
        public string DsMatch { get; set; }
        public string MacAddress { get; set; }

        public ProfileViewModel(DualShockProfile model)
        {
            _model = model;
            Name = model.Name;
            Model = model.Model.ToString();
            DsMatch = model.Match.ToString();
            MacAddress = model.MacAddress;
            foreach (var dsButtonProfile in model.Buttons)
            {
                ButtonMappings.Add(
                    new ButtonMappingViewModel(dsButtonProfile)
                    );
            }
        }

        public ObservableCollection<SourceButtonViewModel> AvailableSourceButtons = new ObservableCollection<SourceButtonViewModel>();
        public ObservableCollection<TargetButtonViewModel> AvailableTargetButtons = new ObservableCollection<TargetButtonViewModel>();

        public ObservableCollection<ButtonMappingViewModel> ButtonMappings { get; set; }

        public void AddMapping()
        {
            ButtonMappings.Add(CurrentMapping);
        }

        public void RemoveMapping(ButtonMappingViewModel mapping)
        {
            if (ButtonMappings.Contains(mapping))
                ButtonMappings.Remove(mapping);
        }

        public ButtonMappingViewModel CurrentMapping { get; set; }
        //public ObservableCollection<SourceAxisProfileViewModel> AxisMappings { get; set; }
        //public ObservableCollection<SourceTouchpadViewModel> TouchpadMappings { get; set; }

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
