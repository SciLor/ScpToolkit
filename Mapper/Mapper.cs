using System;
using Config;
using HidReport.Contract;
using HidReport.Contract.Core;
using HidReport.Contract.Enums;
using Mapper.Contract;
using Mapper.Profiler;

namespace Mapper
{
    class Mapper: IMapper
    {

        private readonly byte[][] _vibration =
        {
            new byte[2] {0, 0}, new byte[2] {0, 0}, new byte[2] {0, 0},
            new byte[2] {0, 0}
        };


        private readonly byte[][] _mNative =
        {
            new byte[2] {0, 0}, new byte[2] {0, 0}, new byte[2] {0, 0},
            new byte[2] {0, 0}
        };

        public void OnHidReportReceived(object sender, IScpHidReport e)
        {

            if (GlobalConfiguration.Instance.ProfilesEnabled)
            {
                // pass current report through user profiles
                DualShockProfileManager.Instance.PassThroughAllProfiles(e);
            }

            //TODO: number
            int serial = 0;
            if (e.PadState == DsState.Connected)
            {
                IMapperProfile profile = null;
                IDsDevice device = null;
                // translate current report to Xbox format and send it to bus device
                // get current pad ID
                XOutputWrapper.Instance.SetState((uint)serial, profile.Map(e));

                // set currently assigned XInput slot
                device.XInputSlot = XOutputWrapper.Instance.GetRealIndex((uint)serial);

                byte largeMotor = 0;
                byte smallMotor = 0;

                // forward rumble request to pad
                if (XOutputWrapper.Instance.GetState((uint)serial, ref largeMotor, ref smallMotor)
                    && (largeMotor != _vibration[serial][0] || smallMotor != _vibration[serial][1]))
                {
                    _vibration[serial][0] = largeMotor;
                    _vibration[serial][1] = smallMotor;

                    device.Rumble(largeMotor, smallMotor);
                }
            }
            else
            {
                // reset rumble/vibration to off state
                _vibration[serial][0] = _vibration[serial][1] = 0;
                _mNative[serial][0] = _mNative[serial][1] = 0;


            }

            // skip broadcast if native feed is disabled
            if (GlobalConfiguration.Instance.DisableNative)
                return;

            // send native controller inputs to subscribed clients
            //TODO
            //foreach (
            //    var channel in _nativeFeedSubscribers.Select(nativeFeedSubscriber => nativeFeedSubscriber.Value))
            //{
            //    try
            //    {
            //        channel.SendAsync(e);
            //    }
            //    catch (AggregateException)
            //    {
            //        /* This might happen if the client disconnects while sending the 
            //         * response is still in progress. The exception can be ignored. */
            //    }
            //}
        }

        public bool Rumble(IDsDevice device, byte large, byte small)
        {
            var serial = GetMappedId(device);
            if (large == _mNative[serial][0] && small == _mNative[serial][1]) return false;

            _mNative[serial][0] = large;
            _mNative[serial][1] = small;

            device.Rumble(large, small);
            return true;
        }

        private int GetMappedId(IDsDevice device)
        {
            throw new NotImplementedException();
        }

        public void OnHidreportReceived(object sender, IScpHidReport e)
        {
            throw new NotImplementedException();
        }
    }
}
