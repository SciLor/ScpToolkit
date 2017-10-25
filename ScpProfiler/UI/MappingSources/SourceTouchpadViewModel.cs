using System.ComponentModel;
using System.Runtime.CompilerServices;
using ScpProfiler.Properties;

namespace ScpProfiler.UI.MappingSources
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