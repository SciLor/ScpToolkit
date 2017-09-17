using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapper
{
    class MapperFeatures
    {
#if zzz
                        if ((now - Last).TotalMilliseconds >= GlobalConfiguration.Instance.Ds3LeDsPeriod
                    && PacketCounter > 0
                    && XInputSlot.HasValue)
                {
                    Last = now;
                    _ledStatus = 0;

                    switch (GlobalConfiguration.Instance.Ds3LeDsFunc)
                    {
                        case 0:
                            _ledStatus = 0;
                            break;
                        case 1:
                            if (GlobalConfiguration.Instance.Ds3PadIdleDsFlashCharging &&
                                Battery == DsBattery.Charging)
                            {
                                _counterForLeds++;
                                _counterForLeds %= 2;
                                if (_counterForLeds == 1)
                                    _ledStatus = _ledOffsets[(int)XInputSlot];
                            }
                            else _ledStatus = _ledOffsets[(int)XInputSlot];
                            break;
                        case 2:
                            switch (Battery)
                            {
                                case DsBattery.None:
                                    _ledStatus = 0;
                                    break;
                                case DsBattery.Charging:
                                    _counterForLeds++;
                                    _counterForLeds %= (byte)_ledOffsets.Length;
                                    for (byte i = 0; i <= _counterForLeds; i++)
                                        _ledStatus |= _ledOffsets[i];
                                    break;
                                case DsBattery.Charged:
                                    _ledStatus =
                                        (byte)(_ledOffsets[0] | _ledOffsets[1] | _ledOffsets[2] | _ledOffsets[3]);
                                    break;
                                default:
                                    ;
                                    break;
                            }
                            break;
                        case 3:
                            if (GlobalConfiguration.Instance.Ds3LEDsCustom1) _ledStatus |= _ledOffsets[0];
                            if (GlobalConfiguration.Instance.Ds3LEDsCustom2) _ledStatus |= _ledOffsets[1];
                            if (GlobalConfiguration.Instance.Ds3LEDsCustom3) _ledStatus |= _ledOffsets[2];
                            if (GlobalConfiguration.Instance.Ds3LEDsCustom4) _ledStatus |= _ledOffsets[3];
                            break;
                        default:
                            _ledStatus = 0;
                            break;
                    }

                    _hidReport[9] = _ledStatus;
                }









                private bool _flash;
                private byte _brightness = GlobalConfiguration.Instance.Brightness;


                        if (!GlobalConfiguration.Instance.IsLightBarDisabled)
                {
                    if (Battery == DsBattery.Dying)
                    {
                        if (!_flash)
                        {
                            _hidReport[12] = _hidReport[13] = 0x40;

                            _flash = true;
                            Queued = 1;
                        }
}
                    else
                    {
                        if (_flash)
                        {
                            _hidReport[12] = _hidReport[13] = 0x00;

                            _flash = false;
                            Queued = 1;
                        }
                    }
                }
                //        if (GlobalConfiguration.Instance.Brightness != _brightness)
                //{
                //    _brightness = GlobalConfiguration.Instance.Brightness;
                //}

                //if (GlobalConfiguration.Instance.Ds4ShowBatteryInfo)
                //{
                //    switch (Battery)
                //    {
                //        case DsBattery.Dying:
                //            SetLightBarColorUInt(GlobalConfiguration.Instance.Ds4ColorDying);
                //            break;
                //        case DsBattery.Low:
                //            SetLightBarColorUInt(GlobalConfiguration.Instance.Ds4ColorLow);
                //            break;
                //        case DsBattery.Medium:
                //            SetLightBarColorUInt(GlobalConfiguration.Instance.Ds4ColorMedium);
                //            break;
                //        case DsBattery.High:
                //        case DsBattery.Charging:
                //            SetLightBarColorUInt(GlobalConfiguration.Instance.Ds4ColorHigh);
                //            break;
                //        case DsBattery.Full:
                //        case DsBattery.Charged:
                //            SetLightBarColorUInt(GlobalConfiguration.Instance.Ds4ColorFull);
                //            break;
                //        default:
                //            SetLightBarColorUInt(GlobalConfiguration.Instance.Ds4ColorDying);
                //            break;
                //    }
                //}
                //else if (XInputSlot.HasValue)
                //{
                //    SetLightBarColor((DsPadId) XInputSlot);
                //}
#endif




        //Mappint to xoutput
        //        if (XOutputWrapper.Instance.PlugIn(_pads.Count))
        //{
        //    _pads.Add(serial);
        //    Log.DebugFormat("-- Bus Plugin : Serial {0}", serial);
        //}
        //else
        //{
        //    Log.ErrorFormat("Couldn't plug in virtual device {0}: {1}", serial,
        //        new Win32Exception(Marshal.GetLastWin32Error()));
        //}

        //if (!GlobalConfiguration.Instance.IsVBusDisabled)
        //{
        //    Log.InfoFormat("Plugged in Port #{0} for {1} on Virtual Bus", (int)arrived.PadId + 1,
        //        arrived.DeviceAddress.AsFriendlyName());
        //}

        //public bool Unplug(IGamepad serial)
        //{
        //    if (GlobalConfiguration.Instance.IsVBusDisabled) return true;

        //    var retVal = false;

        //    lock (_pads)
        //    {
        //        if (_pads.Contains(serial))
        //        {
        //            int id = _pads.IndexOf(serial);
        //            if (XOutputWrapper.Instance.UnPlug(id))
        //            {
        //                _pads.Remove(serial);
        //                retVal = true;

        //                Log.DebugFormat("-- Bus Unplug : Serial {0}", serial);
        //            }
        //            else
        //            {
        //                Log.ErrorFormat("Couldn't unplug virtual device {0}: {1}", serial,
        //                    new Win32Exception(Marshal.GetLastWin32Error()));
        //            }
        //        }
        //        else retVal = true;
        //    }

        //    return retVal;
        //}
    }
}
