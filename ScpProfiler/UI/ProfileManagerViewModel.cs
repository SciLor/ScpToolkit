using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using HidReport.Contract.Core;
using Profiler.Contract;
using ScpControl;
using ScpControl.Shared.Core;
using ScpControl.Utilities;
using ScpProfiler.Model;
using ScpProfiler.Properties;

namespace ScpProfiler.ViewModels
{
    internal class ProfileManagerViewModel: IHidReportNotifier, INotifyPropertyChanged
    {
        private readonly ScpProxy _proxy = new ScpProxy();
        public ProfileManagerViewModel()
        {

            _proxy.NativeFeedReceived += ProxyOnNativeFeedReceived;
            _proxy.Start();
            IEnumerable<DualShockProfile> list;
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

        public ObservableCollection<DeviceViewModel> Devices { get; } = new ObservableCollection<DeviceViewModel>();

        private void ProxyOnNativeFeedReceived(object sender, ScpHidReport scpHidReport)
        {
            if (CurrentProfile == null) return;

            OnHidReportReceived?.Invoke(this, scpHidReport.HidReport);

            var deviceViewModel = new DeviceViewModel()
            {
                MacAddress = scpHidReport.PadMacAddress.AsFriendlyName(),
                Model = scpHidReport.Model,
                PadId = scpHidReport.PadId
            };
            if (Devices.FirstOrDefault(p=>p.MacAddress == deviceViewModel.MacAddress) == null)
                Devices.Add(deviceViewModel);
        }

        public event EventHandler<IScpHidReport> OnHidReportReceived;

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
