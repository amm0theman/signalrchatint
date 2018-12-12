using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSignalR
{
    [HubName("chat")]
    public class ChatHub : Hub, IChatHub
    {
        ObservableCollection<string> connectedUsers;
        ObservableCollection<string> chatLog;

        public ChatHub(ref ObservableCollection<string> _connectedUsers, ref ObservableCollection<string> _chatLog)
        {
            connectedUsers = _connectedUsers;
            chatLog = _chatLog;
        }

        //When they login essentially
        public override Task OnConnected()
        {
            return base.OnConnected();
        }

        //When they disconnect essentially
        public override Task OnDisconnected(bool stopCalled)
        {
            Clients.All.receivedMessage(Context.ConnectionId, " has disconnected");
            Clients.All.receivedUsernameDisconnect(Context.ConnectionId);
            chatLog.Add(Context.ConnectionId + " has disconnected");
            return base.OnDisconnected(stopCalled);
        }

        //sets the username
        public void setName()
        {
            //set local user to this con id
            Clients.Caller.receivedLocalUsername(Context.ConnectionId);

            //get all connected users
            Clients.Caller.receivedUsernames(connectedUsers);

            //Let everyone know you are connected
            Clients.All.receivedUsername(Context.ConnectionId);

            connectedUsers.Add(Context.ConnectionId);
            chatLog.Add(Context.ConnectionId + " joined the chatroom");
        }

        //When they send a message
        public void sendMessage(string user, string message)
        {
            Clients.All.receivedMessage(user, message);
            chatLog.Add(user + ": " + message);
        }

        //When log requested
        public void getLog()
        {
            Clients.Caller.getLog(chatLog);
        }
    }
}
