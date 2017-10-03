using ScpControl.Driver;

namespace ScpControl.Profiler
{
    public interface IFeedbackManager
    {
        void Process(IDsDevice dsDevice);
    }

    internal class FeedbackManager : IFeedbackManager
    {
        private readonly XOutputWrapper _xOutputWrapper;

        private readonly byte[][] _vibration =
        {
            new byte[2] {0, 0}, new byte[2] {0, 0}, new byte[2] {0, 0},
            new byte[2] {0, 0}
        };

        public FeedbackManager(XOutputWrapper xOutputWrapper)
        {
            _xOutputWrapper = xOutputWrapper;
        }

        public void Process(IDsDevice dsDevice)
        {
            ForwardXOutputRequest(dsDevice);
        }

        private void ForwardXOutputRequest(IDsDevice dsDevice)
        {
            var padId = (uint) dsDevice.PadId;
            // set currently assigned XInput slot
            dsDevice.XInputSlot = _xOutputWrapper.GetRealIndex(padId);

            byte largeMotor = 0;
            byte smallMotor = 0;

            // forward rumble request to pad
            if (XOutputWrapper.Instance.GetState(padId, ref largeMotor, ref smallMotor)
                && (largeMotor != _vibration[padId][0] || smallMotor != _vibration[padId][1]))
            {
                _vibration[padId][0] = largeMotor;
                _vibration[padId][1] = smallMotor;

                dsDevice.Rumble(largeMotor, smallMotor);
            }
        }
    }
}