using SignalRChat.Entities.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace SignalRChat.Entities
{
    //Value object that ensures that constraints are put on the string representing a user input message
    public class Message
    {
        public string MessageBody { get; }     //String representing the message text

        public Message(string MessageBody)
        {
            this.MessageBody = MessageBody;
        }
    }
}
