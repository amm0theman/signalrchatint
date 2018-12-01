using System;
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
        public event EventHandler<LogEventArgs> LogReceived;

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
            chatHubProxy.On("getLog", (string log) =>
            {
                OnLogReceived(log);
            });
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

        protected virtual void OnLogReceived(string log)
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
    }

    public class MessageEventArgs : EventArgs
    {
        public string User { get; set; }
        public string Message { get; set; }
    }

    public class LogEventArgs : EventArgs
    {
        public string Log { get; set; }
    }

    public interface IClientHubProxy
    {
        void setName();
        void startHub();
        void sendMessage(string user, string message);
        void getLog();
        event EventHandler<MessageEventArgs> MessageReceived;
        event EventHandler<MessageEventArgs> UsernameReceived;
        event EventHandler<LogEventArgs> LogReceived;
    }
}
