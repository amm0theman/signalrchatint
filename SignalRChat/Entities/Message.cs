using System;
using System.Collections.Generic;
using System.Text;

namespace SignalRChat.Entities
{
    //Value object that ensures that constraints are put on the string representing a user input message
    class Message
    {
        private string MessageBody { get; }     //String representing the message text

        public Message(string MessageBody)
        {
            if(isValidMessage(MessageBody)) this.MessageBody = MessageBody;
        }

        //Ensures message is proper format, sanitized, etc.
        private bool isValidMessage(string message)
        {
            //No messages allowed past 500 characters
            if (message.Length > 500)
            {
                return false;
            }

            if (containsLexicalExpansion(message))
            {
                return false;
            }

            return true;
        }

        //checks for xaml expansion
        private bool containsLexicalExpansion(string message)
        {
            return true;
        }
    }
}
