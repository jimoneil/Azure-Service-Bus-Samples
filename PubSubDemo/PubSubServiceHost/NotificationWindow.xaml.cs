using System;
using System.Windows;
using System.Windows.Media;

namespace PubSubServiceHost
{
    public partial class NotificationWindow : Window
    {
        public NotificationWindow()
        {
            InitializeComponent();
        }

        public NotificationWindow(String userName,  String messageText, 
                                  String backColor, String foreColor) : this()
        {
            Random random = new Random((Int32) System.DateTime.Now.Ticks);
            this.Top = random.Next((int) (System.Windows.SystemParameters.PrimaryScreenHeight - this.Height));
            this.Left = random.Next((int) (System.Windows.SystemParameters.PrimaryScreenWidth - this.Width));

            try
            {
                this.Background = new BrushConverter().ConvertFromString(backColor) as SolidColorBrush;
                this.Foreground = new BrushConverter().ConvertFromString(foreColor) as SolidColorBrush;
            }
            catch (Exception)
            {
                this.Background = new SolidColorBrush(Colors.Black);
                this.Foreground = new SolidColorBrush(Colors.White);
            }

            this.textUser.Text = String.IsNullOrEmpty(userName) ? String.Empty : String.Format("Urgent message from {0}", userName);
            this.textMessage.Text = messageText;
        }

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
