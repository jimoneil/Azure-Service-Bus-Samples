namespace RelayServiceHost
{
    class EchoService : RelayService.Interfaces.IEchoContract
    {
        public void Echo(string userName, string msgText, string colorText)
        {
            var w = new NotificationWindow(userName, msgText, colorText);
            WindowRegistry.Add(w);            
            
            w.Show();
            App.Current.MainWindow.Activate();
        }
    }
}