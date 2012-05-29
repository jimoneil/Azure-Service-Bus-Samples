using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace PubSubServiceHost
{
    public partial class Main : Window
    {
        private Thread backgroundThread = default(Thread);
        private Boolean isProcessing = false;

        public Main()
        {
            InitializeComponent();
        }

        internal void ProcessMessages()
        {
            MessagingFactory factory = MessagingFactory.Create(
                ServiceBusEnvironment.CreateServiceUri("sb",
                    Properties.Settings.Default.SBNamespace,
                    String.Empty),
                TokenProvider.CreateSharedSecretTokenProvider("wpfsample",
                    Properties.Settings.Default.SBListenerCredentials));

            SubscriptionClient urgentMessageClient = 
                factory.CreateSubscriptionClient("thetopic", "urgentmessages");

            while (isProcessing)
            {
                BrokeredMessage message = urgentMessageClient.Receive(new TimeSpan(0, 0, 2));
                if (message != null)
                {
                    Dispatcher.Invoke((System.Action)(() =>
                        {
                            try
                            {
                                var w = new NotificationWindow(
                                    message.Properties["Sender"].ToString(),
                                    message.GetBody<String>(),
                                    message.Properties["BackColor"].ToString(),
                                    message.Properties["ForeColor"].ToString()
                                );
                                WindowRegistry.Add(w);
                                w.Show();
                            }

                            catch (Exception ex)
                            {

                                btnServiceControl.Content = "Start Responding";
                                this.Background = new SolidColorBrush(Colors.Orange);
                                this.isProcessing = false;

                                MessageBox.Show(ex.Message, "Processing halted", MessageBoxButton.OK, MessageBoxImage.Stop);
                            }
                        }
                    ));

                    message.Complete();
                }
            }
        }

        private void btnServiceControl_Click(object sender, RoutedEventArgs e)
        {
            if (!isProcessing)
            {
                backgroundThread = new Thread(this.ProcessMessages);
                backgroundThread.SetApartmentState(ApartmentState.STA);

                backgroundThread.Start();

                btnServiceControl.Content = "Stop Listening";
                this.Background = new SolidColorBrush(Colors.LimeGreen);
                this.isProcessing = true;
            }
            else
            {
                btnServiceControl.Content = "Start Listening";

                WindowRegistry.CloseAll();
                this.Background = new SolidColorBrush(Colors.Silver);
                this.isProcessing = false;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
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

