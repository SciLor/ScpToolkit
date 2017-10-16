using System.Runtime.Serialization;

namespace Profiler.Contract.MappingTargets
{
    [DataContract]
    [KnownType(typeof(MouseAxis))]
    public class MouseAxis : IMappingTarget
    {
        public virtual string Name => "Mouse axis";
    }
}