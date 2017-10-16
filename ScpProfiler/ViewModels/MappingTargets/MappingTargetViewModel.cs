using System.ComponentModel;
using System.Runtime.CompilerServices;
using Profiler.Contract;
using ScpProfiler.Properties;

namespace ScpProfiler.ViewModels.MappingTargets
{
    internal class MappingTargetViewModel : INotifyPropertyChanged
    {
        public MappingTargetViewModel(IMappingTarget buttonProfileMappingTarget)
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