using System;
using System.Windows;
using System.Windows.Media;

namespace BrokeredServiceHost
{
    public partial class NotificationWindow : Window
    {
        public NotificationWindow()
        {
            InitializeComponent();
        }

        public NotificationWindow(String userName, String messageText, String colorText)
            : this()
        {
            Random random = new Random((Int32)System.DateTime.Now.Ticks);
            this.Top = random.Next((int)(System.Windows.SystemParameters.PrimaryScreenHeight - this.Height));
            this.Left = random.Next((int)(System.Windows.SystemParameters.PrimaryScreenWidth - this.Width));

            this.textUser.Text = String.IsNullOrEmpty(userName) ? String.Empty : String.Format("{0} says:", userName);
            this.textMessage.Text = messageText;
            try
            {
                this.Background = new BrushConverter().ConvertFromString(colorText) as SolidColorBrush;
            }
            catch (Exception)
            {
                this.Background = new SolidColorBrush(Colors.White);
            }
        }

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
