namespace Rossie.Windows.Service
{
    partial class RossieEngineServiceInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.rossieServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.rossieServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // rossieServiceProcessInstaller
            // 
            this.rossieServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.rossieServiceProcessInstaller.Password = null;
            this.rossieServiceProcessInstaller.Username = null;
            // 
            // rossieServiceInstaller
            // 
            this.rossieServiceInstaller.ServiceName = "Rossie Engine Service";
            // 
            // RossieEngineServiceInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.rossieServiceProcessInstaller,
            this.rossieServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller rossieServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller rossieServiceInstaller;
    }
}