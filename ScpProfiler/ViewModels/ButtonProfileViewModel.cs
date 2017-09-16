using System.ComponentModel;
using System.Runtime.CompilerServices;
using ScpControl.Shared.Core;

namespace ScpProfiler
{
    internal class ButtonProfileViewModel : INotifyPropertyChanged
    {
        private readonly DsButtonProfile _buttonProfile;

        public ButtonProfileViewModel(DsButtonProfile buttonProfile)
        {
            _buttonProfile = buttonProfile;
            SourceButtonViewModel = new SourceButtonViewModel(buttonProfile.SourceButton);
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