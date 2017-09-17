using Utilites;

namespace ScpDriverInstaller.View_Models
{
    public class InstallationOptionsViewModel
    {
        //TODO: xbox driver check
        public bool IsXbox360DriverNeeded => !(OsInfoHelper.OsParse(OsInfoHelper.OsInfo) >= OsType.Win8);

        public bool InstallDs3ButtonEnabled { get; set; }

        public bool InstallBthButtonEnabled { get; set; }

        public InstallationOptionsViewModel()
        {
            InstallDs3ButtonEnabled = false;
            InstallBthButtonEnabled = false;
        }
    }
}
