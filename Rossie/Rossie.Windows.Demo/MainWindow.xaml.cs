using System;
using System.Globalization;
using System.ServiceModel;
using System.ServiceProcess;
using System.Threading;
using System.Windows;

namespace Rossie.Windows.Demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Execute_Click(object sender, RoutedEventArgs e)
        {
            RunInSandbox(InputCode.Text);
        }
        private static readonly Uri ServiceUri = new Uri("net.pipe://localhost/Pipe");
        private const string PipeName = "RossieEngineService";
        private static readonly EndpointAddress ServiceAddress = new EndpointAddress(string.Format(CultureInfo.InvariantCulture, "{0}/{1}", ServiceUri.OriginalString, PipeName));
        private static ICommandService _serviceProxy;

        private static void StartCodeService()
        {
            var service = new ServiceController("Rossie Engine Service");
            if (service.Status != ServiceControllerStatus.Running)
            {
                service.Start();

                service.WaitForStatus(ServiceControllerStatus.Running);
            }
            _serviceProxy = ChannelFactory<ICommandService>.CreateChannel(new NetNamedPipeBinding(), ServiceAddress);
        }

        private void RunInSandbox(string code)
        {
            StartCodeService();

            var doRestart = false;
            var serviceResult = "timeout";
            var invocationThread = new Thread(() =>
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
                try
                {
                    serviceResult = _serviceProxy.Execute(code);
                }
                catch (EndpointNotFoundException ex)
                {
                    doRestart = true;
                }
                catch (Exception ex)
                {
                    _serviceProxy = null;
                }
            });

            invocationThread.Start();

            invocationThread.Join(6000);

            if (doRestart)
            {
                _serviceProxy = null;
                RunInSandbox(code);
            }
            else
            {
                if (string.IsNullOrEmpty(serviceResult)) serviceResult = "null";
                ResultView.Text = serviceResult;
            }
        }
    }
}
