using System.ComponentModel;
using System.Runtime.CompilerServices;
using ScpControl.Shared.Core;
using ScpProfiler.Properties;

namespace ScpProfiler.UI
{
    internal class DeviceViewModel:INotifyPropertyChanged
    {
        public DsPadId PadId { get; set; }
        public DsModel Model { get; set; }
        public string MacAddress { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}