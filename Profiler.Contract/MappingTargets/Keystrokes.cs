using System.Runtime.Serialization;
using WindowsInput.Native;

namespace Profiler.Contract.MappingTargets
{
    [DataContract]
    [KnownType(typeof(Keystrokes))]
    public class Keystrokes : IMappingTarget
    {
        public Keystrokes(VirtualKeyCode code)
        {
            Code = code;
        }

        public VirtualKeyCode Code { get; private set; }
        public virtual string Name => "Keystrokes";

    }
}