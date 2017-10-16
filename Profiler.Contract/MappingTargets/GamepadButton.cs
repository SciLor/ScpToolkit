using System.Runtime.Serialization;
using ScpControl.Shared.Core;

namespace Profiler.Contract.MappingTargets
{
    [DataContract]
    [KnownType(typeof(GamepadButton))]
    public class GamepadButton : IMappingTarget
    {
        public GamepadButton(X360Button button)
        {
            Button = button;
        }

        public X360Button Button { get; private set; }
        public virtual string Name => "Gamepad buttons";
    }
}