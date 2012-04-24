using System.ServiceModel;

namespace RelayService.Interfaces
{
    [ServiceContract(Name = "IEchoContract", 
                     Namespace = "http://samples.microsoft.com/ServiceModel/Relay/")]
    public interface IEchoContract
    {
        [OperationContract]
        void Echo(string sender, string text, string color);
    }

    public interface IEchoChannel : IEchoContract, IClientChannel { }
}
