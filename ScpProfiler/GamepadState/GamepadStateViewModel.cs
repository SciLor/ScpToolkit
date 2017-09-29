using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using HidReport.Contract.Core;
using HidReport.Contract.Enums;
using ScpProfiler.Annotations;
using ScpProfiler.ViewModels.Gamepad;

namespace ScpProfiler.ViewModels
{
    internal class GamepadStateViewModel : INotifyPropertyChanged
    {
        private readonly IHidReportNotifier _notifier;

        public GamepadStateViewModel()
            :this(null)
        {
            Debug.Assert(LicenseManager.UsageMode == LicenseUsageMode.Designtime);
            Buttons.Add(new SourceButtonViewModel(ButtonsEnum.Circle, _notifier));
            Buttons.Add(new SourceButtonViewModel(ButtonsEnum.Cross, _notifier));
            Buttons.Add(new SourceButtonViewModel(ButtonsEnum.Triangle, _notifier));
            Buttons.Add(new SourceButtonViewModel(ButtonsEnum.Square, _notifier));
            Buttons.Add(new SourceButtonViewModel());
            Axes.Add(new SourceAxisViewModel(AxesEnum.Circle, _notifier));
            Axes.Add(new SourceAxisViewModel(AxesEnum.Lx, _notifier));
            Axes.Add(new SourceAxisViewModel(AxesEnum.Ly, _notifier));
            Axes.Add(new SourceAxisViewModel(AxesEnum.Rx, _notifier));
            Axes.Add(new SourceAxisViewModel(AxesEnum.Ry, _notifier));
        }

        public GamepadStateViewModel([NotNull] IHidReportNotifier notifier)
        {
            _notifier = notifier;
            if(_notifier != null)
                _notifier.OnHidReportReceived += NotifierOnOnHidReportReceived;
        }

        private static readonly ButtonsEnum[] AllButtons = {
            ButtonsEnum.Select,
            ButtonsEnum.L3,
            ButtonsEnum.R3,
            ButtonsEnum.Start,
            ButtonsEnum.Up,
            ButtonsEnum.Right,
            ButtonsEnum.Down,
            ButtonsEnum.Left,
            ButtonsEnum.L2,
            ButtonsEnum.R2,
            ButtonsEnum.L1,
            ButtonsEnum.R1,
            ButtonsEnum.Triangle,
            ButtonsEnum.Circle,
            ButtonsEnum.Cross,
            ButtonsEnum.Square,
            ButtonsEnum.Ps,
            ButtonsEnum.Share,
            ButtonsEnum.Options,
            ButtonsEnum.Touchpad,
        };

        private static readonly AxesEnum[] AllAxes =
        {
            AxesEnum.Lx,
            AxesEnum.Ly,
            AxesEnum.Rx,
            AxesEnum.Ry,
            AxesEnum.Up,
            AxesEnum.Right,
            AxesEnum.Down,
            AxesEnum.Left,
            AxesEnum.L2,
            AxesEnum.R2,
            AxesEnum.L1,
            AxesEnum.R1,
            AxesEnum.Triangle,
            AxesEnum.Circle,
            AxesEnum.Cross,
            AxesEnum.Square,
        };

        private void NotifierOnOnHidReportReceived(object sender, IScpHidReport scpHidReport)
        {
            //TODO: device should say which buttons it have

            foreach (var button in AllButtons)
            {
                var state = scpHidReport[button];
                if (state == null)
                    continue;
                if (_buttons.ContainsKey(button))
                    continue;
                var sourceButtonViewModel = new SourceButtonViewModel(button, _notifier);
                _buttons.Add(button, sourceButtonViewModel);
                Buttons.Add(sourceButtonViewModel);
            }
            foreach (var axis in AllAxes)
            {
                var state = scpHidReport[axis];
                if (state == null)
                    continue;
                if (_axes.ContainsKey(axis))
                    continue;
                var sourceButtonViewModel = new SourceAxisViewModel(axis, _notifier);
                _axes.Add(axis, sourceButtonViewModel);
                Axes.Add(sourceButtonViewModel);
            }
            //TODO: add other input types
        }

        private readonly Dictionary<ButtonsEnum, SourceButtonViewModel> _buttons =
            new Dictionary<ButtonsEnum, SourceButtonViewModel>();

        private readonly Dictionary<AxesEnum, SourceAxisViewModel> _axes =
            new Dictionary<AxesEnum, SourceAxisViewModel>();

        public ObservableCollection<SourceButtonViewModel> Buttons { get; } = new ObservableCollection<SourceButtonViewModel>();
        public ObservableCollection<SourceAxisViewModel> Axes { get; } = new ObservableCollection<SourceAxisViewModel>();
        public ObservableCollection<SourceTouchpadViewModel> Touchpads { get; } = new ObservableCollection<SourceTouchpadViewModel>();
        public ObservableCollection<SourceGyroViewModel> Gyros { get; } = new ObservableCollection<SourceGyroViewModel>();
        public ObservableCollection<SourceMotionViewModel> Motions { get; } = new ObservableCollection<SourceMotionViewModel>();

        //TODO: make orientation viewModel for 3d
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}