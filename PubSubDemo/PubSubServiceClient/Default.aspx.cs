using System;
using System.Configuration;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace PubSubServiceClient
{
    public partial class Default : System.Web.UI.Page
    {
        protected void btnSend_Click(object sender, EventArgs e)
        {
            if (txtMessage.Text.Trim().Length == 0) return;
            String userName = (txtUser.Text.Trim().Length == 0) ? "guest" : txtUser.Text;

            // create the message and set properties
            BrokeredMessage message = new BrokeredMessage(txtMessage.Text);
            message.Properties["Sender"] = txtUser.Text;
            message.Properties["Priority"] = Int32.Parse(ddlPriority.SelectedValue);

            // send the message
            MessagingFactory factory = MessagingFactory.Create(
                ServiceBusEnvironment.CreateServiceUri("sb",
                ConfigurationManager.AppSettings["SBNamespace"],
                String.Empty),
                TokenProvider.CreateSharedSecretTokenProvider(
                    userName,
                    ConfigurationManager.AppSettings["SBGuestCredentials"]));
            factory.CreateTopicClient("thetopic").Send(message);
        }
    }
}