﻿using System;
using System.Runtime.InteropServices;
using RGiesecke.DllExport;

namespace ScpXInputBridge
{
    public partial class XInputDll
    {
        #region SCP extension function

        [DllExport("XInputGetExtended", CallingConvention.StdCall)]
        public static uint XInputGetExtended(uint dwUserIndex, ref SCP_EXTN pPressure)
        {
            try
            {
                pPressure = Proxy.GetExtended(dwUserIndex);
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("Couldn't receive pressure information: {0}", ex);
            }

            return 0; // success
        }

        #endregion
    }
}
