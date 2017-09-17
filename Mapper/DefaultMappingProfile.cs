using Config;
using HidReport.Contract.Core;
using HidReport.Contract.Enums;
using Mapper.Contract;
using ScpXInputBridge.XInputConstants;

namespace Mapper
{
    internal class DefaultMappingProfile : IMapperProfile
    {
        public XINPUT_GAMEPAD Map(IScpHidReport inputReport)
        {
            var xButton = X360Button.None;

            if (inputReport[ButtonsEnum.Up].IsPressed) xButton |= X360Button.Up;
            if (inputReport[ButtonsEnum.Right].IsPressed) xButton |= X360Button.Right;
            if (inputReport[ButtonsEnum.Down].IsPressed) xButton |= X360Button.Down;
            if (inputReport[ButtonsEnum.Left].IsPressed) xButton |= X360Button.Left;

            if (inputReport[ButtonsEnum.L1].IsPressed) xButton |= X360Button.LB;
            if (inputReport[ButtonsEnum.R1].IsPressed) xButton |= X360Button.RB;

            if (inputReport[ButtonsEnum.Triangle].IsPressed) xButton |= X360Button.Y;
            if (inputReport[ButtonsEnum.Circle].IsPressed) xButton |= X360Button.B;
            if (inputReport[ButtonsEnum.Cross].IsPressed) xButton |= X360Button.A;
            if (inputReport[ButtonsEnum.Square].IsPressed) xButton |= X360Button.X;

            if (inputReport[ButtonsEnum.L3].IsPressed) xButton |= X360Button.LS;
            if (inputReport[ButtonsEnum.R3].IsPressed) xButton |= X360Button.RS;

            if (inputReport[ButtonsEnum.Share].IsPressed) xButton |= X360Button.Back;
            if (inputReport[ButtonsEnum.Options].IsPressed) xButton |= X360Button.Start;

            var output = new XINPUT_GAMEPAD();
            // face buttons
            output.wButtons = (ushort)xButton;



            //if(ps3)
            {
                if (inputReport[ButtonsEnum.Ps].IsPressed) xButton |= X360Button.Guide;
                if (inputReport[ButtonsEnum.Select].IsPressed) xButton |= X360Button.Back;
                if (inputReport[ButtonsEnum.Start].IsPressed) xButton |= X360Button.Start;

            }
            //else
            {
                // PS/Guide
                if (inputReport[ButtonsEnum.Ps].IsPressed) xButton |= X360Button.Guide;
            }

            // trigger
            output.bLeftTrigger = inputReport[AxesEnum.L2].Value;
            output.bRightTrigger = inputReport[AxesEnum.R2].Value;

            if (!DsMath.DeadZone(GlobalConfiguration.Instance.DeadZoneL,
                    inputReport[AxesEnum.Lx].Value,
                    inputReport[AxesEnum.Ly].Value))
                // Left Stick DeadZone
            {
                output.sThumbLX =
                    (short)
                    +DsMath.Scale(inputReport[AxesEnum.Lx].Value, GlobalConfiguration.Instance.FlipLX);
                output.sThumbLY =
                    (short)
                    -DsMath.Scale(inputReport[AxesEnum.Ly].Value, GlobalConfiguration.Instance.FlipLY);
            }

            if (!DsMath.DeadZone(GlobalConfiguration.Instance.DeadZoneR,
                    inputReport[AxesEnum.Rx].Value,
                    inputReport[AxesEnum.Ry].Value))
                // Right Stick DeadZone
            {
                output.sThumbRX =
                    (short)
                    +DsMath.Scale(inputReport[AxesEnum.Rx].Value, GlobalConfiguration.Instance.FlipRX);
                output.sThumbRY =
                    (short)
                    -DsMath.Scale(inputReport[AxesEnum.Ry].Value, GlobalConfiguration.Instance.FlipRY);
            }
            return output;
        }
    }
}