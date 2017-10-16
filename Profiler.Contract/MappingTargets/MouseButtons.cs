using System.Runtime.Serialization;
using WindowsInput;

namespace Profiler.Contract.MappingTargets
{
    [DataContract]
    [KnownType(typeof(MouseButtons))]
    public class MouseButtons : IMappingTarget
    {
        public MouseButton Button { get; private set; }
        public virtual string Name => "Mouse buttons";
    }
}