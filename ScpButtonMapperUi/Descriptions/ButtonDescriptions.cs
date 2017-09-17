using System;
using HidReport.Contract.Enums;

namespace ScpProfiler.Descriptions
{
    internal static class ButtonDescriptions
    {
        public static ButtonDescription Get(ButtonsEnum button)
        {
            switch (button)
            {
                case ButtonsEnum.Select  : return new ButtonDescription("Select"  );
                case ButtonsEnum.L3      : return new ButtonDescription("L3"      ) { DisplayName = "Left thumb" };
                case ButtonsEnum.R3      : return new ButtonDescription("R3"      ) { DisplayName = "Right thumb" };
                case ButtonsEnum.Start   : return new ButtonDescription("Start"   );
                case ButtonsEnum.Up      : return new ButtonDescription("Up"      ) { DisplayName = "D-Pad up"      };
                case ButtonsEnum.Right   : return new ButtonDescription("Right"   ) { DisplayName = "D-Pad right"   };
                case ButtonsEnum.Down    : return new ButtonDescription("Down"    ) { DisplayName = "D-Pad down"    };
                case ButtonsEnum.Left    : return new ButtonDescription("Left"    ) { DisplayName = "D-Pad left"    };
                case ButtonsEnum.L2      : return new ButtonDescription("L2"      ) { DisplayName = "Left trigger"  };
                case ButtonsEnum.R2      : return new ButtonDescription("R2"      ) { DisplayName = "Right trigger" };
                case ButtonsEnum.L1      : return new ButtonDescription("L1"      ) { DisplayName = "Left shoulder" };
                case ButtonsEnum.R1      : return new ButtonDescription("R1"      ) { DisplayName = "Right shoulder"};
                case ButtonsEnum.Triangle: return new ButtonDescription("Triangle") ;
                case ButtonsEnum.Circle  : return new ButtonDescription("Circle"  ) ;
                case ButtonsEnum.Cross   : return new ButtonDescription("Cross"   ) ;
                case ButtonsEnum.Square  : return new ButtonDescription("Square"  ) ;
                case ButtonsEnum.Ps      : return new ButtonDescription("PS"      ) ;
                case ButtonsEnum.Share   : return new ButtonDescription("Share")    ;
                case ButtonsEnum.Options : return new ButtonDescription("Options")  ;
                case ButtonsEnum.Touchpad: return new ButtonDescription("Touchpad");
                default:
                    throw new ArgumentOutOfRangeException(nameof(button), button, null);
            }
        }
    }
}