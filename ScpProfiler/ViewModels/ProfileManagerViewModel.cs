using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using ScpControl;
using ScpControl.Shared.Core;
using ScpProfiler.Annotations;

namespace ScpProfiler
{
    internal class ProfileManagerViewModel: INotifyPropertyChanged
    {
        private readonly ScpProxy _proxy = new ScpProxy();
        public ProfileManagerViewModel()
        {

            _proxy.NativeFeedReceived += ProxyOnNativeFeedReceived;
            _proxy.Start();
            IEnumerable<DualShockProfile> list = null;
            try
            {
                list = _proxy.GetProfiles();
            }
            catch (Exception err)
            {
                throw new Exception($"Can't load profiles. Error {err.Message}", err);
            }

            if (!list.Any())
            {
            _proxy.SubmitProfile(DualShockProfile.DefaultProfile());
            }
            try
            {
                list = _proxy.GetProfiles();
            }
            catch (Exception err)
            {
                throw new Exception($"Can't load profiles. Error {err.Message}", err);
            }
            Debug.Assert(list != null);

            Profiles = new ObservableCollection<ProfileViewModel>(
                list.Select(p=>new ProfileViewModel(p)));
            CurrentProfile = Profiles.First();
        }

        public ObservableCollection<DeviceViewModel> Devices { get; }
        public DeviceViewModel DeviceViewModel { get; }

        private void ProxyOnNativeFeedReceived(object sender, ScpHidReport scpHidReport)
        {
            if (CurrentProfile == null) return;

            if (scpHidReport.PadId != DeviceViewModel.PadId) return;

            DeviceViewModel.Model = scpHidReport.Model;
            DeviceViewModel.MacAddress = string.Join(":",
                (from z in scpHidReport.PadMacAddress.GetAddressBytes() select z.ToString("X2")).ToArray());
            DeviceViewModel.PadId = scpHidReport.PadId;
        }
        public ProfileViewModel CurrentProfile { get; set; }

        public ObservableCollection<ProfileViewModel> Profiles { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
