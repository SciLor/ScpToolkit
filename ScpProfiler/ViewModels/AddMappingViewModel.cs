using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScpProfiler.ViewModels
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
