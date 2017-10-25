using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using HidReport.Contract.Core;
using HidReport.Contract.Enums;
using ScpProfiler.Model;
using ScpProfiler.Properties;

namespace ScpProfiler.UI.MappingSources
{
    internal struct SourceButtonViewSettings
    {
        public string Name;
        public string IconSource;

        public SourceButtonViewSettings(ButtonsEnum button)
        {
            //TODO: button description should come from device
            const string basePath = "../Icons/Gamepad/";
            switch (button)
            {
                case ButtonsEnum.Select:
                    IconSource = basePath + "select.png";
                    Name = "Select";
                    break;
                case ButtonsEnum.L3:
                    IconSource = basePath + "lstick_none.png";
                    Name = "Left thumb";
                    break;
                case ButtonsEnum.R3:
                    IconSource = basePath + "rstick_none.png";
                    Name = "Right thumb";
                    break;
                case ButtonsEnum.Start:
                    IconSource = basePath + "start.png";
                    Name = "Start";
                    break;
                case ButtonsEnum.Up:
                    IconSource = basePath + "dpad_up.png";
                    Name = "D-Pad up";
                    break;
                case ButtonsEnum.Right:
                    IconSource = basePath + "dpad_right.png";
                    Name = "D-Pad right";
                    break;
                case ButtonsEnum.Down:
                    IconSource = basePath + "dpad_down.png";
                    Name = "D-Pad down";
                    break;
                case ButtonsEnum.Left:
                    IconSource = basePath + "dpad_left.png";
                    Name = "D-Pad left";
                    break;
                case ButtonsEnum.L2:
                    IconSource = basePath + "l2.png";
                    Name = "Left trigger";
                    break;
                case ButtonsEnum.R2:
                    IconSource = basePath + "r2.png";
                    Name = "Right trigger";
                    break;
                case ButtonsEnum.L1:
                    IconSource = basePath + "l1.png";
                    Name = "Left shoulder";
                    break;
                case ButtonsEnum.R1:
                    IconSource = basePath + "r1.png";
                    Name = "Right shoulder";
                    break;
                case ButtonsEnum.Triangle:
                    IconSource = basePath + "triangle.png";
                    Name = "Triangle";
                    break;
                case ButtonsEnum.Circle:
                    IconSource = basePath + "circle.png";
                    Name = "Circle";
                    break;
                case ButtonsEnum.Cross:
                    IconSource = basePath + "cross.png";
                    Name = "Cross";
                    break;
                case ButtonsEnum.Square:
                    IconSource = basePath + "square.png";
                    Name = "Square";
                    break;
                case ButtonsEnum.Ps:
                    IconSource = basePath + "home.png";
                    Name = "PS Home button";
                    break;
                case ButtonsEnum.Share:
                    IconSource = basePath + "share.png"; ;
                    Name = "Share button";
                    break;
                case ButtonsEnum.Options:
                    IconSource = basePath + "options.png"; ;
                    Name = "Options button";
                    break;
                case ButtonsEnum.Touchpad:
                    IconSource = null;
                    Name = "Touchpad touch event";
                    //TODO: create touchpad icon
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(button), button, null);
            }
        }
    }

    internal class MappingSourceViewModel : INotifyPropertyChanged
    {
        private readonly IHidReportNotifier _notifier;
        private readonly ButtonsEnum _buttonId;

        public MappingSourceViewModel()
            : this(ButtonsEnum.Ps, null)
        {
            Debug.Assert(LicenseManager.UsageMode == LicenseUsageMode.Designtime);
            IsPressed = true;
        }

        public MappingSourceViewModel(ButtonsEnum button, [CanBeNull] IHidReportNotifier notifier)
        {
            _buttonId = button;
            _notifier = notifier;
            var viewSettings = new SourceButtonViewSettings(button);
            IconSource = viewSettings.IconSource;
            Name = viewSettings.Name;
            if (_notifier != null)
                _notifier.OnHidReportReceived += NotifierOnOnHidReportReceived;
        }

        private void NotifierOnOnHidReportReceived(object sender, IScpHidReport scpHidReport)
        {
            var state = scpHidReport[_buttonId];
            if (state != null)
            {
                IsPressed = state.IsPressed;
            }
        }

        public string IconSource { get; }
        public string Name { get; }
        public bool IsPressed { get; private set; }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}