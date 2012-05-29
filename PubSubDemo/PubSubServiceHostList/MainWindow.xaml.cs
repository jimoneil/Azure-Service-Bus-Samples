using System;
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

            SubscriptionClient allMessageClient =
                factory.CreateSubscriptionClient("thetopic", "allmessages");

            while (isProcessing)
            {
                BrokeredMessage message = allMessageClient.Receive(new TimeSpan(0, 0, 2));
                if (message != null)
                {
                    Dispatcher.Invoke((System.Action)(()
                        =>
                    {
                        try
                        {
                            lbMessages.Items.Add(String.Format("{0}{1}: {2}",
                                message.Properties["Sender"].ToString(),
                                new[] { String.Empty, "!", "*" }
                                    [Int32.Parse(message.Properties["Priority"].ToString())],
                                message.GetBody<String>()));

                        }
                        catch (Exception ex)
                        {
                            lbMessages.Items.Add(String.Format("Exception: {0} {1}",
                                message.MessageId, ex.Message));
                            this.isProcessing = false;
                        }
                    }));

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

                lbMessages.Items.Clear();
                this.Background = new SolidColorBrush(Colors.Silver);
                this.isProcessing = false;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            lbMessages.Items.Clear();
        }
    }
}