using System.Drawing;
using System.Security.Cryptography.X509Certificates;

namespace HidReport.Contract.Core
{
    public interface IScpFeedback
    {
        byte RumbleBig { get; }
        byte RumbleSmall { get; }
        Color LightBarColor { get; }
        byte Pad4Lights { get; }
    }
}