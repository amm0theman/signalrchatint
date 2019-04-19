using System;
using System.Collections.Generic;
using System.Text;

namespace SignalRChat.Value_Objects
{
    public class Password
    {
        private string key;

        public Password(string key)
        {
            this.key = key;
        }

        public string getOneTimePassword()
        {
            var temp = key;
            key = "";
            return temp;
        }
    }
}
