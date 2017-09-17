using WindowsInput;
using Utilites;

namespace Mapper.Profiler
{
    public class VirtualMouse : SingletonBase<VirtualMouse>
    {
        private readonly InputSimulator _inputSimulator = new InputSimulator();
        private readonly IMouseSimulator _mouseSimulator;

        private VirtualMouse()
        {
            _mouseSimulator = _inputSimulator.Mouse;
        }
    }
}
