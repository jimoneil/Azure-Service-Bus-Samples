using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace BrokeredServiceHost
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
            try
            {
                MessagingFactory factory = MessagingFactory.Create(
                    ServiceBusEnvironment.CreateServiceUri("sb",
                        Properties.Settings.Default.SBNamespace,
                        String.Empty),
                    TokenProvider.CreateSharedSecretTokenProvider("wpfsample",
                            Properties.Settings.Default.SBListenerCredentials));
                MessageReceiver theQueue = factory.CreateMessageReceiver("thequeue");

                while (isProcessing)
                {
                    BrokeredMessage message = theQueue.Receive(new TimeSpan(0, 0, 0, 5));
                    if (message != null)
                    {
                        Dispatcher.Invoke((System.Action)(()
                            =>

                        {
                            NotificationWindow w;
                            try
                            {
                                w = new NotificationWindow(
                                    message.Properties["Sender"].ToString(),
                                    message.GetBody<String>(),
                                    message.Properties["Color"].ToString());
                            }
                            catch (KeyNotFoundException)
                            {
                                w = new NotificationWindow(
                                    "system",
                                    String.Format("Invalid message:\n{0}", message.GetBody<String>()),
                                    "Red"
                                );
                            }
                            WindowRegistry.Add(w);
                            w.Show();
                            message.Complete();
                        }));
                    }
                }
            }

            catch (Exception ex)
            {
                Dispatcher.Invoke((System.Action)(()
                    =>
                    {
                        btnServiceControl.Content = "Start Responding";
                        this.Background = new SolidColorBrush(Colors.Orange);
                        this.isProcessing = false;
                    }));
                MessageBox.Show(ex.Message, "Processing halted", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }

        private void btnServiceControl_Click(object sender, RoutedEventArgs e)
        {

            if (!isProcessing)
            {
                backgroundThread = new Thread(this.ProcessMessages);
                backgroundThread.SetApartmentState(ApartmentState.STA);

                backgroundThread.Start();

                btnServiceControl.Content = "Stop Responding";
                this.Background = new SolidColorBrush(Colors.LimeGreen);
                this.isProcessing = true;
            }
            else
            {
                btnServiceControl.Content = "Start Responding";

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