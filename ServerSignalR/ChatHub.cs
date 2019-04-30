using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SignalRChat.Entities.Validation;
using SignalRChat.Value_Objects;
using SignalRChat.Aggregates;
using System.Data.SQLite;

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
            connectedUsers.Remove(Context.ConnectionId);
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
        public void sendMessage(Parcel message)
        {
            // TODO: Validate Message
            Clients.All.receivedMessage(message);
            chatLog.Add(message.Owner.ToString() + ": " + message);
        }

        public void getLogin(User user)
        {
            
            bool matched = CreateDB.LoginUser(user.UserName.ToString(), user.PassWord.ToString());

            //If so, login, else error
            if (matched)
            {
                Clients.Caller.confirmLogin(true);
            }
            else
            {
                Clients.Caller.confirmLogin(false);
            }
        }

        public void getSignup(User user)
        {
            //Check to see if there are no users like this
            //Send encrypted user info to the server
            //Login command\
            bool matched = CreateDB.SignUpUser(user.UserName.ToString(), user.PassWord.ToString());

            if (matched)
            {
                Clients.Caller.confirmSignup(true);
            }
            else
            {
                Clients.Caller.confirmSignup(false);
            }
        }

        //When log requested
        public void getLog()
        {
            Clients.Caller.getLog(chatLog);
        }
    }
}
