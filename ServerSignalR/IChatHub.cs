using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSignalR
{
    interface IChatHub
    {
        void getLog();
        void sendMessage(string user, string message);
        Task OnConnected();
        Task OnDisconnected(bool stopCalled);
    }
}
