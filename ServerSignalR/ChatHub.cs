using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSignalR
{
    [HubName("chat")]
    public class ChatHub : Hub, IChatHub
    {
        //When they login essentially
        public override Task OnConnected()
        {
            return base.OnConnected();
        }

        //When they disconnect essentially
        public override Task OnDisconnected(bool stopCalled)
        {
            return base.OnDisconnected(stopCalled);
        }

        //sets the username
        public void setName()
        {
            Clients.All.receivedUsername(Context.ConnectionId);
        }

        //When they send a message
        public void sendMessage(string user, string message)
        {
            Clients.All.receivedMessage(user, message);
        }

        //When log requested
        public void getLog()
        {
            Clients.All.receivedMessage("ChatBot |O o|", "Not yet implemented");
            //Clients.Caller.receiveMessage("ChatBot |O o|", DB.getlog);
        }
    }
}
