using System.ComponentModel;
using System.Runtime.CompilerServices;
using ScpProfiler.Annotations;

namespace ScpProfiler
{
    internal class SourceTouchpadViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}