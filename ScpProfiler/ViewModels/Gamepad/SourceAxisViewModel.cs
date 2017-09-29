using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using HidReport.Contract.Core;
using HidReport.Contract.Enums;
using ScpProfiler.Annotations;

namespace ScpProfiler
{
    internal class SourceAxisViewModel : INotifyPropertyChanged
    {
        private readonly AxesEnum _axis;
        private readonly IHidReportNotifier _notifier;

        public SourceAxisViewModel()
            :this(AxesEnum.L1, null)
        {
            Debug.Assert(LicenseManager.UsageMode == LicenseUsageMode.Designtime);
            Value = 200;
        }

        public SourceAxisViewModel(AxesEnum axis, [NotNull] IHidReportNotifier notifier)
        {
            _axis = axis;
            _notifier = notifier;
            Debug.Assert(_notifier !=null);
            if(_notifier != null)
                _notifier.OnHidReportReceived += NotifierOnOnHidReportReceived;

            const string basePath = "../Icons/Gamepad/";
            switch (axis)
            {
                case AxesEnum.Up:
                    IconSource = basePath+"dpad_up.png";
                    Name = "D-Pad up";
                    break;
                case AxesEnum.Right:
                    IconSource = basePath+"dpad_right.png";
                    Name = "D-Pad right";
                    break;
                case AxesEnum.Down:
                    IconSource = basePath+"dpad_down.png";
                    Name = "D-Pad down";
                    break;
                case AxesEnum.Left:
                    IconSource = basePath+"dpad_left.png";
                    Name = "D-Pad left";
                    break;
                case AxesEnum.L2:
                    IconSource = basePath+"l2.png";
                    Name = "Left trigger";
                    break;
                case AxesEnum.R2:
                    IconSource = basePath+"r2.png";
                    Name = "Right trigger";
                    break;
                case AxesEnum.L1:
                    IconSource = basePath+"l1.png";
                    Name = "Left shoulder";
                    break;
                case AxesEnum.R1:
                    IconSource = basePath+"r1.png";
                    Name = "Right shoulder";
                    break;
                case AxesEnum.Triangle:
                    IconSource = basePath+"triangle.png";
                    Name = "Triangle";
                    break;
                case AxesEnum.Circle:
                    IconSource = basePath+"circle.png";
                    Name = "Circle";
                    break;
                case AxesEnum.Cross:
                    IconSource = basePath+"cross.png";
                    Name = "Cross";
                    break;
                case AxesEnum.Square:
                    IconSource = basePath+"sqaure.png";
                    Name = "Square";
                    break;
                case AxesEnum.Lx:
                    IconSource = basePath + "lstick_leftright.png";
                    Name = "Left stick X";
                    break;
                case AxesEnum.Ly:
                    IconSource = basePath + "lstick_updown.png";
                    Name = "Left stick Y";
                    break;
                case AxesEnum.Rx:
                    IconSource = basePath + "rstick_leftright.png";
                    Name = "Right stick X";
                    break;
                case AxesEnum.Ry:
                    IconSource = basePath + "rstick_updown.png";
                    Name = "Right stick Y";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(axis), axis, null);
            }
        }

        private void NotifierOnOnHidReportReceived(object sender, IScpHidReport scpHidReport)
        {
            var state = scpHidReport[_axis];
            Debug.Assert(state != null);
            //TODO: normalize value at lower level
            if (state != null)
                Value = state.Value;
        }

        public string Name { get; }
        public string IconSource { get; }

        public int Value { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}