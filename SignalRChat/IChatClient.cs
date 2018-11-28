using System;
using System.Collections.Generic;
using System.Text;

namespace SignalRChat
{
    public interface IChatClient
    {
        string Username { get; set; }
        string Log { get; set; }
        void receivedMessage(object source, MessageEventArgs e);
        void sendMessage(string user, string message);
        void receivedUsername(object sender, MessageEventArgs e);
    }
}
