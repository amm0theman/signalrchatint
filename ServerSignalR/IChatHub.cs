using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SignalRChat.Aggregates;

namespace ServerSignalR
{
    interface IChatHub
    {
        void getLog();
        void sendMessage(Parcel user);
        Task OnConnected();
        Task OnDisconnected(bool stopCalled);
    }
}
