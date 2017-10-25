using System.Collections.ObjectModel;

namespace ScpProfiler.UI
{
    class AddMappingViewModel
    {
        public ObservableCollection<string> Targets { get; } = new ObservableCollection<string>()
        {
            "Gamepad button",
            "Keystroke",
            "Mouse button",
            "Mouse movement"
        };

    }
}
