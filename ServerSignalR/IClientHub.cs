using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSignalR
{
    public interface IClientHub
    {
        void receiveMessage(string a, string b);
        void receiveLog();
        void getName();
    }
}
