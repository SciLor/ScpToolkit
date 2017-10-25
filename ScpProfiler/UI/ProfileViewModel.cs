using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Profiler.Contract;
using ScpProfiler.Properties;
using ScpProfiler.UI.MappingSources;
using ScpProfiler.UI.MappingTargets;

namespace ScpProfiler.UI
{
    internal class ProfileViewModel : INotifyPropertyChanged
    {
        private readonly DualShockProfile _profile;
        public string Name { get; set; }
        public string Model { get; set; }
        public string DsMatch { get; set; }
        public string MacAddress { get; set; }

        public ProfileViewModel()
        {
            Name = "Test";
            Model = "DsModel.DS4";
            MacAddress = "Test mac";

            ButtonMappings = new ObservableCollection<MappingViewModel>()
            {
                new MappingViewModel(),
                new MappingViewModel(),
                new MappingViewModel(),
                new MappingViewModel(),
            };
        }

        public ProfileViewModel(DualShockProfile profile)
        {
            _profile = profile;
            Name = profile.Name;
            Model = profile.Model.ToString();
            DsMatch = profile.Match.ToString();
            MacAddress = profile.MacAddress;
            foreach (var dsButtonProfile in profile.Buttons)
            {
                ButtonMappings.Add(
                    new MappingViewModel(dsButtonProfile)
                    );
            }
        }

        public ObservableCollection<SourceButtonViewModel> AvailableSourceButtons = new ObservableCollection<SourceButtonViewModel>();
        public ObservableCollection<MappingTargetViewModelBase> AvailableTargetButtons = new ObservableCollection<MappingTargetViewModelBase>();

        public ObservableCollection<MappingViewModel> ButtonMappings { get; set; }

        public void AddMapping()
        {
            ButtonMappings.Add(CurrentMapping);
        }

        public void RemoveMapping(MappingViewModel mapping)
        {
            if (ButtonMappings.Contains(mapping))
                ButtonMappings.Remove(mapping);
        }

        public MappingViewModel CurrentMapping { get; set; }
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
