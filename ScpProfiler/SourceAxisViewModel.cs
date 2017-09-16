using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using HidReport.Contract.Enums;
using ScpProfiler.Annotations;

namespace ScpProfiler
{
    internal class SourceAxisViewModel : INotifyPropertyChanged
    {
        public SourceAxisViewModel(AxesEnum axis)
        {
            switch (axis)
            {
                case AxesEnum.Up:
                    IconSource = "Icons/Ds4/dpad_up.png";
                    Name = "D-Pad up";
                    break;
                case AxesEnum.Right:
                    IconSource = "Icons/Ds4/dpad_right.png";
                    Name = "D-Pad right";
                    break;
                case AxesEnum.Down:
                    IconSource = "Icons/Ds4/dpad_down.png";
                    Name = "D-Pad down";
                    break;
                case AxesEnum.Left:
                    IconSource = "Icons/Ds4/dpad_left.png";
                    Name = "D-Pad left";
                    break;
                case AxesEnum.L2:
                    IconSource = "Icons/48px-PS3_L2.png";
                    Name = "Left trigger";
                    break;
                case AxesEnum.R2:
                    IconSource = "Icons/48px-PS3_R2.png";
                    Name = "Right trigger";
                    break;
                case AxesEnum.L1:
                    IconSource = "Icons/48px-PS3_L1.png";
                    Name = "Left shoulder";
                    break;
                case AxesEnum.R1:
                    IconSource = "Icons/48px-PS3_R1.png";
                    Name = "Right shoulder";
                    break;
                case AxesEnum.Triangle:
                    IconSource = "Icons/48px-PS3_Triangle.png";
                    Name = "Triangle";
                    break;
                case AxesEnum.Circle:
                    IconSource = "Icons/48px-PS3_Circle.png";
                    Name = "Circle";
                    break;
                case AxesEnum.Cross:
                    IconSource = "Icons/48px-PS3_Cross.png";
                    Name = "Cross";
                    break;
                case AxesEnum.Square:
                    IconSource = "Icons/48px-PS3_Sqaure.png";
                    Name = "Square";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(axis), axis, null);
            }
        }

        public string Name { get; }
        public string IconSource { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}