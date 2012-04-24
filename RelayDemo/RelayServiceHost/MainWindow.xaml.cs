using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.ServiceBus;

namespace RelayServiceHost
{
    public partial class Main : Window
    {
        public ServiceHost host = default(ServiceHost);

        public Main()
        {
            InitializeComponent();
        }

        private void btnServiceControl_Click(object sender, RoutedEventArgs e)
        {  
            this.Cursor = Cursors.Wait;

            if (!IsServerRunning())
            {
                try
                {
                    // instantiate ServiceHost based on UI setting
                    host = (bool) cbUseServiceBus.IsChecked ? GetServiceBusHost() : GetLocalWASHost();

                    host.Open();
                    if (host.State == CommunicationState.Opened)
                    {
                        this.Background = new SolidColorBrush(Colors.LimeGreen);
                        btnServiceControl.Content = "Stop Service";
                    }
                    else
                        this.Background = new SolidColorBrush(Colors.Yellow);
                }
                catch (Exception ex)
                {
                    this.Background = new SolidColorBrush(Colors.Tomato);
                    btnServiceControl.Content = "Start Service";

                    MessageBox.Show(ex.Message, "Service Not Started");
                }
            }
            else
            {
                if (host.State == CommunicationState.Opened)
                {
                    host.Close();

                    WindowRegistry.CloseAll();

                    this.Background = new SolidColorBrush(Colors.Silver);
                    btnServiceControl.Content = "Start Service";
                }
            }
            
            cbUseServiceBus.IsEnabled = !IsServerRunning();
            this.Cursor = Cursors.Arrow;
        }

        private ServiceHost GetLocalWASHost()
        {
            ServiceHost host = default(ServiceHost);

            // local WCF service end point  
            host = new ServiceHost(typeof(EchoService));
            host.AddServiceEndpoint("RelayService.Interfaces.IEchoContract",
                new BasicHttpBinding(),
                "http://localhost:7777/EchoService");

            return host;
        }

        private ServiceHost GetServiceBusHost()
        {
            ServiceHost host = default(ServiceHost);

            // ServiceBusEndpoint
            host = new ServiceHost(typeof(EchoService));
            host.AddServiceEndpoint("RelayService.Interfaces.IEchoContract",
                new BasicHttpRelayBinding(),
                ServiceBusEnvironment.CreateServiceUri("https",
                    Properties.Settings.Default.SBNamespace,
                    "EchoService"));
           
            // Add the Service Bus credentials to all endpoints specified in configuration.
            foreach (ServiceEndpoint endpoint in host.Description.Endpoints)
            {
                endpoint.Behaviors.Add(new TransportClientEndpointBehavior()
                    {
                        TokenProvider = TokenProvider.CreateSharedSecretTokenProvider(
                            "wpfsample",
                            Properties.Settings.Default.SBListenerCredentials)
                    });
            }

            return host;
        }

        private Boolean IsServerRunning()
        {
            return (host != null) && ((host.State == CommunicationState.Opened) || (host.State == CommunicationState.Created) || (host.State == CommunicationState.Opening));
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            host.Close();
            WindowRegistry.CloseAll();
        }
    }

    internal static class WindowRegistry
    {
        private static List<NotificationWindow> Windows = new List<NotificationWindow>();

        internal static void Add(NotificationWindow w)
        {
            Windows.Add(w);
        }

        internal static void CloseAll()
        {
            foreach (var w in Windows)
                w.Close();
            Windows.Clear();
        }
    }
}

