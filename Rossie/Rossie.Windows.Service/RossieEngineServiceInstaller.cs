using System.ComponentModel;


namespace Rossie.Windows.Service
{
    [RunInstaller(true)]
    public partial class RossieEngineServiceInstaller : System.Configuration.Install.Installer
    {
        public RossieEngineServiceInstaller()
        {
            InitializeComponent();
        }
    }
}
