using System.ComponentModel;
using System.Runtime.CompilerServices;
using ScpControl.Shared.Core;
using ScpProfiler.Annotations;

namespace ScpProfiler
{
    internal class TargetButtonViewModel : INotifyPropertyChanged
    {
        public TargetButtonViewModel(IMappingTarget buttonProfileMappingTarget)
        {
            //throw new System.NotImplementedException();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}