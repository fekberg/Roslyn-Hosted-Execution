using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;

namespace Rossie.Windows.Service
{
    public partial class RossieEngineService : ServiceBase
    {
        public RossieEngineService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                if (!EventLog.SourceExists("RossieEngineService"))
                    EventLog.CreateEventSource("RossieEngineService", "Application");

                EventLog.WriteEntry("RossieEngineService", "Starting Command Server", EventLogEntryType.Information);

var thread = new Thread(CommandServer.Start);

thread.Start();
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("RossieEngineService", ex.ToString(), EventLogEntryType.Error);
            }
        }

        protected override void OnStop()
        {
            CommandServer.Stop();
        }
    }
}
