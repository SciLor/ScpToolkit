using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NativeLayer.UsbDevices
{
    public static class BitUtils
    {
        public static bool IsBitSet(byte value, int offset)
        {
            return ((value >> offset) & 1) == 0x01;
        }
    }
}
