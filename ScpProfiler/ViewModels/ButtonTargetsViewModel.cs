using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ScpProfiler.Annotations;

namespace ScpProfiler
{
    internal class ButtonTargetsViewModel : INotifyPropertyChanged
    {

        public ObservableCollection<string> Targets { get; } = new ObservableCollection<string>()
        {
            "Gamepad button",
            "Keystroke",
            "Mouse button",
            "Mouse movement"
        };

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}