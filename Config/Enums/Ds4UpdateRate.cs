using System.Collections.Generic;

namespace Config.Enums
{
    /// <summary>
    ///     Supported HID input update rates.
    /// </summary>
    public enum Ds4UpdateRate : byte
    {
        Fastest = 0x80, // 1000 Hz
        Fast = 0xD0, // 66 Hz
        Slow = 0xA0, // 31 Hz
        Slowest = 0xB0 // 20 Hz
    }

    //TODO:move to UI
    public static class Helper
    {


        /// <summary>
        ///     Supported HID input update rates.
        /// </summary>
        public static Dictionary<Ds4UpdateRate, string> UpdateRates => new Dictionary<Ds4UpdateRate, string>
        {
            {Ds4UpdateRate.Fastest, "1000 Hz"},
            {Ds4UpdateRate.Fast, "66 Hz"},
            {Ds4UpdateRate.Slow, "31 Hz"},
            {Ds4UpdateRate.Slowest, "20 Hz"}
        };
    }
}