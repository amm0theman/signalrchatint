using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Client;

namespace SignalRChat
{
    /*    
        interface IClientHub
        {
            void receiveMessage(string a, string b);
            void receiveLog();
            string getName();
        }
    */
    public class ClientHubProxy : IClientHubProxy
    {
        HubConnection hubConnection;
        IHubProxy chatHubProxy;

        public event EventHandler<MessageEventArgs> MessageReceived;
        public event EventHandler<MessageEventArgs> UsernameReceived;
        public event EventHandler<UsersArgs> UsernamesReceived;
        public event EventHandler<LogEventArgs> LogReceived;
        public event EventHandler<MessageEventArgs> LocalUsernameReceived;
        public event EventHandler<MessageEventArgs> UsernameReceivedDisconnect;

        public ClientHubProxy(string url, string hubType)
        {
            hubConnection = new HubConnection(url);
            chatHubProxy = hubConnection.CreateHubProxy(hubType);
            chatHubProxy.On("receivedMessage", (string user, string message) =>
            {
                OnMessageReceived(user, message);
            });
            chatHubProxy.On("receivedUsername", (string username) =>
            {
                OnUsernameReceived(username);
            });
            chatHubProxy.On("receivedUsernames", (ObservableCollection<string> usernames) =>
            {
                OnUsernamesReceived(usernames);
            });
            chatHubProxy.On("receivedUsernameDisconnect", (string username) =>
            {
                OnUsernameReceivedDisconnect(username);
            });
            chatHubProxy.On("getLog", (ObservableCollection<string> log) =>
            {
                OnLogReceived(log);
            });
            chatHubProxy.On("receivedLocalusername", (string username) =>
            {
                OnReceivedLocalUsername(username);
            });
        }

        protected virtual void OnReceivedLocalUsername(string username)
        {
            if (LocalUsernameReceived != null)
                LocalUsernameReceived(this, new MessageEventArgs() { User = username, Message = "" });
        }

        protected virtual void OnMessageReceived(string user, string message)
        {
            if (MessageReceived != null)
                MessageReceived(this, new MessageEventArgs() { User = user, Message = message });
        }

        protected virtual void OnUsernameReceived(string username)
        {
            if (UsernameReceived != null)
                UsernameReceived(this, new MessageEventArgs() { User = username });
        }

        protected virtual void OnUsernameReceivedDisconnect(string username)
        {
            if (UsernameReceived != null)
                UsernameReceivedDisconnect(this, new MessageEventArgs() { User = username });
        }

        protected virtual void OnUsernamesReceived(ObservableCollection<string> usernames)
        {
            if (UsernameReceived != null)
                UsernamesReceived(this, new UsersArgs() { Users = usernames });
        }

        protected virtual void OnLogReceived(ObservableCollection<string> log)
        {
            if (LogReceived != null)
                LogReceived(this, new LogEventArgs() { Log = log });
        }

        public void setName()
        {
            chatHubProxy.Invoke("setName");
        }

        public void startHub()
        {
            hubConnection.Start().Wait();
        }

        public void sendMessage(string user, string message)
        {
            chatHubProxy.Invoke("sendMessage", user, message);
        }

        public void getLog()
        {
            chatHubProxy.Invoke("getLog");
        }

        public void CloseConnection()
        {
            hubConnection.Stop();
            hubConnection.Dispose();
        }
    }

    public class MessageEventArgs : EventArgs
    {
        public string User { get; set; }
        public string Message { get; set; }
    }

    public class UsersArgs : EventArgs
    {
        public ObservableCollection<string> Users { get; set; }
    }

    public class LogEventArgs : EventArgs
    {
        public ObservableCollection<string> Log { get; set; }
    }

    public interface IClientHubProxy
    {
        void setName();
        void startHub();
        void sendMessage(string user, string message);
        void getLog();
        void CloseConnection();
        event EventHandler<MessageEventArgs> MessageReceived;
        event EventHandler<MessageEventArgs> UsernameReceived;
        event EventHandler<LogEventArgs> LogReceived;
        event EventHandler<UsersArgs> UsernamesReceived;
        event EventHandler<MessageEventArgs> LocalUsernameReceived;
        event EventHandler<MessageEventArgs> UsernameReceivedDisconnect;
    }
}
