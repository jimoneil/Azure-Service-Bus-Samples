using System;
using System.Configuration;
using System.ServiceModel;
using Microsoft.ServiceBus;
using RelayService.Interfaces;

namespace RelayServiceClient
{
    public partial class Default : System.Web.UI.Page
    {
        private void InvokeLocal(string userName)
        {
            ChannelFactory<IEchoContract> myChannelFactory =
                new ChannelFactory<IEchoContract>(
                    new BasicHttpBinding(), "http://localhost:7777/EchoService");

            IEchoContract client = myChannelFactory.CreateChannel();
            client.Echo(userName, txtMessage.Text, ddlColors.SelectedItem.Text);

            ((IClientChannel)client).Close();
            myChannelFactory.Close();
        }

        private void InvokeServiceBus(String userName)
        {
            // ServiceBus endpoint constructed using App.config setting
            ChannelFactory<IEchoContract> myChannelFactory =
                new ChannelFactory<IEchoContract>(
                    new BasicHttpRelayBinding(),
                    new EndpointAddress(ServiceBusEnvironment.CreateServiceUri(
                        "https", 
                        ConfigurationManager.AppSettings["SBNameSpace"], 
                        "EchoService")));

            // add credentials for user (hardcoded in App.config for demo purposes)
            myChannelFactory.Endpoint.Behaviors.Add(
                new TransportClientEndpointBehavior()
                {
                    TokenProvider = TokenProvider.CreateSharedSecretTokenProvider(
                        userName, ConfigurationManager.AppSettings["SBGuestCredentials"])
                }
            );

            // traditional WCF invocation
            IEchoContract client = myChannelFactory.CreateChannel();
            client.Echo(txtUser.Text, txtMessage.Text, ddlColors.SelectedItem.Text);

            ((IClientChannel)client).Close();
            myChannelFactory.Close();
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            // default sender to "guest" and don't send if empty message
            if (txtMessage.Text.Trim().Length == 0) return;
            String userName = (txtUser.Text.Trim().Length == 0) ? "guest" : txtUser.Text;

            // call locally or via ServiceBus depending on selection
            try
            {
                if (rbServiceType.SelectedValue == "0")
                    InvokeLocal(userName);
                else
                    InvokeServiceBus(userName);
            }

            // WCF endpoint not found
            catch (EndpointNotFoundException eex)
            {
                Response.Write(String.Format("<code style='color: red' width='100%'>EndpointNotFoundException: {0}</code>",
                    eex.Message));
            }

            // Service Bus endpoint not found
            catch (FaultException fex)
            {
                Response.Write(String.Format("<code style='color: red' width='100%'>FaultException: {0}</code>",
                     fex.Message));
            }

            // Service Bus Authentication failed
            catch (System.IdentityModel.Tokens.SecurityTokenException sex)
            {
                Response.Write(String.Format("<code style='color: red'>SecurityTokenException: {0}</code>",
                    sex.InnerException.Message));
            }

            catch (Exception ex)
            {
                Response.Write(String.Format("<code style='color: red'>{0}</code><pre style='color: red'>{1}</pre>", ex.Message, ex.StackTrace));
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            rbServiceType.Items[0].Text = "Via WCF Service (http://localhost:7777/EchoService)";
            rbServiceType.Items[1].Text =
                String.Format("Via ServiceBus (http://{0}.servicebus.windows.net/EchoService)",
                ConfigurationManager.AppSettings["SBNamespace"]);
        }
    }
}