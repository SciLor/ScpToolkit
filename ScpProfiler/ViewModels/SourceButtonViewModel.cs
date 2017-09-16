using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using HidReport.Contract.Enums;
using ScpProfiler.Annotations;

namespace ScpProfiler
{
    internal class SourceButtonViewModel : INotifyPropertyChanged
    {
        public SourceButtonViewModel(ButtonsEnum button)
        {
            switch (button)
            {
                case ButtonsEnum.Select:
                    IconSource = "Icons/48px-PS3_Select.png";
                    Name = "Select";
                    break;
                case ButtonsEnum.L3:
                    IconSource = "Icons/48px-PS3_L3.png";
                    Name = "Left thumb";
                    break;
                case ButtonsEnum.R3:
                    IconSource = "Icons/48px-PS3_R3.png";
                    Name = "Right thumb";
                    break;
                case ButtonsEnum.Start:
                    IconSource = "Icons/48px-PS3_Start.png";
                    Name = "Start";
                    break;
                case ButtonsEnum.Up:
                    IconSource = "Icons/Ds4/dpad_up.png";
                    Name = "D-Pad up";
                    break;
                case ButtonsEnum.Right:
                    IconSource = "Icons/Ds4/dpad_right.png";
                    Name = "D-Pad right";
                    break;
                case ButtonsEnum.Down:
                    IconSource = "Icons/Ds4/dpad_down.png";
                    Name = "D-Pad down";
                    break;
                case ButtonsEnum.Left:
                    IconSource = "Icons/Ds4/dpad_left.png";
                    Name = "D-Pad left";
                    break;
                case ButtonsEnum.L2:
                    IconSource = "Icons/48px-PS3_L2.png";
                    Name = "Left trigger";
                    break;
                case ButtonsEnum.R2:
                    IconSource = "Icons/48px-PS3_R2.png";
                    Name = "Right trigger";
                    break;
                case ButtonsEnum.L1:
                    IconSource = "Icons/48px-PS3_L1.png";
                    Name = "Left shoulder";
                    break;
                case ButtonsEnum.R1:
                    IconSource = "Icons/48px-PS3_R1.png";
                    Name = "Right shoulder";
                    break;
                case ButtonsEnum.Triangle:
                    IconSource = "Icons/48px-PS3_Triangle.png";
                    Name = "Triangle";
                    break;
                case ButtonsEnum.Circle:
                    IconSource = "Icons/48px-PS3_Circle.png";
                    Name = "Circle";
                    break;
                case ButtonsEnum.Cross:
                    IconSource = "Icons/48px-PS3_Cross.png";
                    Name = "Cross";
                    break;
                case ButtonsEnum.Square:
                    IconSource = "Icons/48px-PS3_Sqaure.png";
                    Name = "Square";
                    break;
                case ButtonsEnum.Ps:
                    IconSource = "Icons/48px-PS3_PSHome.png";
                    Name = "PS Home button";
                    break;
                case ButtonsEnum.Share:
                    break;
                case ButtonsEnum.Options:
                    break;
                case ButtonsEnum.Touchpad:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(button), button, null);
            }

        }

        public string IconSource { get; }
        public string Name { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}