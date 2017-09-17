using System;

namespace ScpXInputBridge.XInputConstants
{
    #region Native structs

    // These are only valid for wireless, connected devices, with known battery types
    // The amount of use time remaining depends on the type of device.

    public class XInputConstants
    {
        public const int XINPUT_DEVTYPE_GAMEPAD = 0x01;

        //
        // Device subtypes available in XINPUT_CAPABILITIES
        //
        public const int XINPUT_DEVSUBTYPE_GAMEPAD = 0x01;

        //
        // Flags for XINPUT_CAPABILITIES
        //
        [Flags]
        public enum CapabilityFlags
        {
            XINPUT_CAPS_VOICE_SUPPORTED = 0x0004,
            //For Windows 8 only
            XINPUT_CAPS_FFB_SUPPORTED = 0x0001,
            XINPUT_CAPS_WIRELESS = 0x0002,
            XINPUT_CAPS_PMD_SUPPORTED = 0x0008,
            XINPUT_CAPS_NO_NAVIGATION = 0x0010,
        };
        //
        // Constants for gamepad buttons
        //

        //
        // Gamepad thresholds
        //
        public const int XINPUT_GAMEPAD_LEFT_THUMB_DEADZONE = 7849;
        public const int XINPUT_GAMEPAD_RIGHT_THUMB_DEADZONE = 8689;
        public const int XINPUT_GAMEPAD_TRIGGER_THRESHOLD = 30;

        //
        // Flags to pass to XInputGetCapabilities
        //
        public const int XINPUT_FLAG_GAMEPAD = 0x00000001;
    }

    #endregion
}
