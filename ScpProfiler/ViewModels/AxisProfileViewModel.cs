using System.ComponentModel;
using System.Runtime.CompilerServices;
using ScpProfiler.Annotations;

namespace ScpProfiler
{
    internal class AxisProfileViewModel : INotifyPropertyChanged
    {
        public AxisProfileViewModel(SourceAxisViewModel sourceAxisViewModel)
        {
            SourceAxisViewModel = sourceAxisViewModel;
        }

        public SourceAxisViewModel SourceAxisViewModel { get; }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}