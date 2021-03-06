﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Client;
using SignalRChat.Aggregates;

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
        public event EventHandler<LoggedEventArgs> LoggedIn;
        public event EventHandler<SignedEventArgs> SignedUp;

        public ClientHubProxy(string url, string hubType)
        {
            hubConnection = new HubConnection(url);
            chatHubProxy = hubConnection.CreateHubProxy(hubType);
            chatHubProxy.On("receivedMessage", (Parcel message) =>
            {
                OnMessageReceived(message);
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
            chatHubProxy.On("getLogin", (bool loggedIn) =>
            {
                OnLoggedIn(loggedIn);
            });
            chatHubProxy.On("getSignup", (bool signedUp) =>
            {
                OnSignedUp(signedUp);
            });
        }

        protected virtual void OnLoggedIn(bool loggedIn)
        {
            if (loggedIn != null)
            {
                LoggedIn(this, new LoggedEventArgs() { status = loggedIn });
                
            }
        }

        protected virtual void OnSignedUp(bool signedUp)
        {
            if (signedUp != null)
            {
                SignedUp(this, new SignedEventArgs() { status = signedUp });

            }
        }


        protected virtual void OnReceivedLocalUsername(string username)
        {
            if (LocalUsernameReceived != null)
                LocalUsernameReceived(this, new MessageEventArgs() { User = username, Message = "" });
        }

        protected virtual void OnMessageReceived(Parcel message)
        {
            if (MessageReceived != null)
                MessageReceived(this, new MessageEventArgs() { User = message.Owner.UserName.username, Message = message.message.MessageBody });
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

        public void sendMessage(Parcel message)
        {
            chatHubProxy.Invoke("sendMessage", message);
        }

        public void getLog()
        {
            chatHubProxy.Invoke("getLog");
        }

        public void getLogin(User user)
        {
            chatHubProxy.Invoke("getLogin", user);
        }

        public void getSignUp(User user)
        {
            chatHubProxy.Invoke("getSignup", user);
        }

        public void CloseConnection()
        {
            hubConnection.Stop();
            hubConnection.Dispose();
        }
    }

    public class SignedEventArgs : EventArgs
    {
        public bool status { get; set; }
    }

    public class LoggedEventArgs : EventArgs
    {
        public bool status { get; set; }
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
        void sendMessage(Parcel message);
        void getLog();
        void CloseConnection();
        void getLogin(User user);
        void getSignUp(User user);
        event EventHandler<MessageEventArgs> MessageReceived;
        event EventHandler<MessageEventArgs> UsernameReceived;
        event EventHandler<LogEventArgs> LogReceived;
        event EventHandler<UsersArgs> UsernamesReceived;
        event EventHandler<MessageEventArgs> LocalUsernameReceived;
        event EventHandler<MessageEventArgs> UsernameReceivedDisconnect;
        event EventHandler<LoggedEventArgs> LoggedIn;
        event EventHandler<SignedEventArgs> SignedUp;
    }
}
