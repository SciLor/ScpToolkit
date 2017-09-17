using System.ComponentModel;

namespace ScpService 
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer 
    {
        public ProjectInstaller() 
        {
            InitializeComponent();
        }
    }
}
