using HidReport.Contract.DsActors;

namespace HidReport.DsActors
{
    public class DsTrackPadTouch : IDsTrackPadTouchImmutable
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}
