using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;


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
