using System.Collections.Generic;
using System.Linq;
using WindowsInput;
using HidReport.Contract.Core;
using HidReport.Contract.Enums;
using ScpControl.ScpCore;
using ScpControl.Shared.Core;
using ScpControl.Shared.Utilities;
using ScpControl.Shared.XInput;

namespace ScpControl
{
    internal class Mapper
    {
        private static readonly InputSimulator VirtualInput = new InputSimulator();

        public Mapper()
        {
            
        }

        /// <summary>
        ///     Translates an <see cref="ScpHidReport"/> to an Xbox 360 compatible byte array.
        /// </summary>
        /// <param name="inputReport">The <see cref="ScpHidReport"/> to translate.</param>
        /// <returns>The translated data as <see cref="XINPUT_GAMEPAD"/> structure.</returns>
        public XINPUT_GAMEPAD MapAxes(IScpHidReport inputReport)
        {
            // trigger
            _output.bLeftTrigger = inputReport[AxesEnum.L2].Value;
            _output.bRightTrigger = inputReport[AxesEnum.R2].Value;

            if (!DsMath.DeadZone(GlobalConfiguration.Instance.DeadZoneL,
                inputReport[AxesEnum.Lx].Value,
                inputReport[AxesEnum.Ly].Value))
            // Left Stick DeadZone
            {
                _output.sThumbLX =
                    (short)
                        +DsMath.Scale(inputReport[AxesEnum.Lx].Value, GlobalConfiguration.Instance.FlipLX);
                _output.sThumbLY =
                    (short)
                        -DsMath.Scale(inputReport[AxesEnum.Ly].Value, GlobalConfiguration.Instance.FlipLY);
            }

            if (!DsMath.DeadZone(GlobalConfiguration.Instance.DeadZoneR,
                inputReport[AxesEnum.Rx].Value,
                inputReport[AxesEnum.Ry].Value))
            // Right Stick DeadZone
            {
                _output.sThumbRX =
                    (short)
                        +DsMath.Scale(inputReport[AxesEnum.Rx].Value, GlobalConfiguration.Instance.FlipRX);
                _output.sThumbRY =
                    (short)
                        -DsMath.Scale(inputReport[AxesEnum.Ry].Value, GlobalConfiguration.Instance.FlipRY);
            }

            return Output;
        }

        /// <summary>
        ///     Applies button re-mapping to the supplied report.
        /// </summary>
        /// <param name="report">The report to manipulate.</param>
        /// <param name="profile"></param>
        private void Remap(IScpHidReport report, DsButtonProfile profile)
        {
            var button = profile.SourceButton;
            if (report[button] == null) return;

            // turbo is special, apply first
            if (profile.Turbo.IsEnabled)
            {
                profile.Turbo.ApplyOn(report, button);
            }

            if (profile.MappingTarget is GamepadButton)
            {
                // get target button
                GamepadButton target = profile.MappingTarget as GamepadButton;
                if (report[button].IsPressed) _xButton |= target.Button;
            }
            else if (profile.MappingTarget is Keystrokes)
            {
                var target = profile.MappingTarget as Keystrokes;

                if (report[button].IsPressed)
                {
                    VirtualInput.Keyboard.KeyDown(target.Code);
                }
                else
                {
                    VirtualInput.Keyboard.KeyUp(target.Code);
                }
            }
        }

        /// <summary>
        ///     Feeds the supplied HID report through all loaded mapping profiles.
        /// </summary>
        /// <param name="report">The extended HID report.</param>
        /// <param name="profiles"></param>
        public void PassThroughAllProfiles(ScpHidReport report, IEnumerable<DualShockProfile> profiles)
        {
            _output = new XINPUT_GAMEPAD();
            _xButton = X360Button.None;
            try
            {
                foreach (DualShockProfile profile in profiles.Where(p => p.IsActive))
                {
                    // determine if profile should be applied
                    switch (profile.Match)
                    {
                        case DsMatch.Global:
                            // always apply
                            break;
                        case DsMatch.Mac:
                            // applies of MAC address matches
                            var reportMac = report.PadMacAddress.ToString();
                            if (string.CompareOrdinal(profile.MacAddress.Replace(":", string.Empty), reportMac) != 0) return;
                            break;
                        case DsMatch.None:
                            // never apply
                            return;
                        case DsMatch.Pad:
                            // applies if pad IDs match
                            if (profile.PadId != report.PadId) return;
                            break;
                    }

                    // walk through all buttons
                    foreach (DsButtonProfile buttonProfile in profile.Buttons)
                    {
                        Remap(report.HidReport, buttonProfile);
                    }
                }
            }
            catch // TODO: remove!
            {
            }
            MapAxes(report.HidReport);
            _output.wButtons = (ushort)_xButton;
        }

        public XINPUT_GAMEPAD Output => _output;

        private X360Button _xButton;
        private XINPUT_GAMEPAD _output;
    }
}
