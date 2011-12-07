using System.ServiceProcess;

namespace Rossie.Windows.Service
{
    static class Program
    {
        static void Main()
        {
            var servicesToRun = new ServiceBase[] 
                                              { 
                                                  new RossieEngineService() 
                                              };
            ServiceBase.Run(servicesToRun);
        }
    }
}
