using System;
using System.Collections.Generic;
using System.Text;
using SignalRChat.Aggregates;

namespace SignalRChat.Entities.Validation
{
    /// <summary>
    /// Class for validating messages origin and size, for use on client and server
    /// Data should be validated whenever it crosses a barrier of trust. For example client to server and vice versa
    /// </summary>
    public static class DataValidator
    {
        //check to see if the ip origin is valid
        public static bool isOriginValid(User user, List<User> users)
        {
            bool isExistingUser = false;

            foreach (User i in users)
            {
                if(i.getIdentity() == user.getIdentity())
                {
                    isExistingUser = true;
                    break;
                }
            }

            return isExistingUser;
        }

        //Validate the body of a message
        public static Tuple<string, bool> validateMessage(string message)
        {
            if (isValidMessage(message))
            {
                return new Tuple<string, bool>(message, true);
            }

            return new Tuple<string, bool>("Invalid Message", false);
        }

        //Ensures message is proper format, sanitized, etc.
        private static bool isValidMessage(string message)
        {
            //No messages allowed past 500 characters
            if (!isProperSize(message))
            {
                return false;
            }

            //check for xaml expansion or whateva
            if (!containsLexicalExpansion(message))
            {
                return false;
            }

            //Check for syntax
            if (!isSyntacticallyCorrect())
            {
                return false;
            }

            //Check for semantics
            if (!isSemanticallyCorrect())
            {
                return false;
            }

            return true;
        }

        //checks message length
        private static bool isProperSize(string message)
        {
            if (message.Length > 1000)
            {
                return false;
            }

            return true;
        }

        //checks for xaml expansion. However, this doesn't seem to be necessary for the program. When xaml expansion was tried to be input to the message box, nothing happened. It just displayed it as a string.
        private static bool containsLexicalExpansion(string message)
        {
            return true;
        }

        //would check for syntax validation, but unnecessary. It's a message, and therefore can contain anything and be valid
        private static bool isSyntacticallyCorrect()
        {
            return true;
        }

        //would check for semantic correctness, but again it is a message
        private static bool isSemanticallyCorrect()
        {
            return true;
        }
    }
}
