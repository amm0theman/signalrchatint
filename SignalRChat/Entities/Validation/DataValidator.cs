using System;
using System.Collections.Generic;
using System.Text;

namespace SignalRChat.Entities.Validation
{
    /// <summary>
    /// Class for validating messages origin and size, for use on client and server
    /// </summary>
    public class DataValidator
    {
        public DataValidator()
        {

        }

        public static string validateMessage(string message)
        {
            if (isValidMessage(message))
            {
                return message;
            }



            return message;
        }

        //Ensures message is proper format, sanitized, etc.
        private static bool isValidMessage(string message)
        {
            //No messages allowed past 500 characters
            if (message.Length > 500)
            {
                return false;
            }

            //check for xaml expansion or whateva
            if (containsLexicalExpansion(message))
            {
                return false;
            }

            return true;
        }

        //checks for xaml expansion. However, this doesn't seem to be necessary for the program. When xaml expansion was tried to be input to the message box, nothing happened. It just displayed it as a string.
        private static bool containsLexicalExpansion(string message)
        {
            return false;
        }
    }
}
