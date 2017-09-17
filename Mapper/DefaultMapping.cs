using HidReport.Contract.Enums;

namespace Mapper
{
    internal static class DefaultMapping
    {
        public static X360Button? Map(ButtonsEnum button)
        {
            if (Equals(button, ButtonsEnum.Select))return X360Button.Back;
            if (Equals(button, ButtonsEnum.L3))return X360Button.LS;
            if (Equals(button, ButtonsEnum.R3))return X360Button.RS;
            if (Equals(button, ButtonsEnum.Start))return X360Button.Back;
            if (Equals(button, ButtonsEnum.Up))return X360Button.Up;
            if (Equals(button, ButtonsEnum.Right))return X360Button.Right;
            if (Equals(button, ButtonsEnum.Down))return X360Button.Down;
            if (Equals(button, ButtonsEnum.Left))return X360Button.Left;

            if (Equals(button, ButtonsEnum.L1))return X360Button.LB;
            if (Equals(button, ButtonsEnum.R1))return X360Button.RB;
            if (Equals(button, ButtonsEnum.Triangle))return X360Button.Y;
            if (Equals(button, ButtonsEnum.Circle))return X360Button.B;
            if (Equals(button, ButtonsEnum.Cross))return X360Button.A;
            if (Equals(button, ButtonsEnum.Square))return X360Button.X;
            if (Equals(button, ButtonsEnum.Ps))return X360Button.Guide;

            if (Equals(button, ButtonsEnum.L2))return null;
            if (Equals(button, ButtonsEnum.R2))return null;
            //PS4
            if (Equals(button, ButtonsEnum.Share))return X360Button.Back;
            if (Equals(button, ButtonsEnum.Options))return X360Button.Start;
            if (Equals(button, ButtonsEnum.Touchpad))return null;
            return null;
        }
    }
}
