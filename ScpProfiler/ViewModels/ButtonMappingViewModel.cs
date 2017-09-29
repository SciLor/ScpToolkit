using System.ComponentModel;
using System.Runtime.CompilerServices;
using ScpControl.Shared.Core;

namespace ScpProfiler
{
    internal class ButtonMappingViewModel : INotifyPropertyChanged
    {
        private readonly DsButtonProfile _buttonProfile;

        public ButtonMappingViewModel(DsButtonProfile buttonProfile)
        {
            _buttonProfile = buttonProfile;
            //TODO: pass notifier
            SourceButtonViewModel = new SourceButtonViewModel(buttonProfile.SourceButton, null);
            TargetButtonViewModel = new TargetButtonViewModel(buttonProfile.MappingTarget);
        }

        public SourceButtonViewModel SourceButtonViewModel { get; }
        public TargetButtonViewModel TargetButtonViewModel { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}