using SignalRChat.Value_Objects;
using SignalRChat.Entities;
using System.Net;
using System.Net.Sockets;
using System.Linq;

namespace SignalRChat.Aggregates
{
    public class User : IOwner
    {
        private Username userName;
        private Password key;
        
        public User(Username UserName, Password PassWord)
        {
            this.userName = UserName;
            this.key = PassWord;
        }

        public Username UserName => userName;
        public Password PassWord => key;

        public IPAddress getIdentity()
        {
            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                return null;
            }

            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

            return host
                .AddressList
                .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
        }
    }
}
