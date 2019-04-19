using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace SignalRChat.Entities
{
    public interface IOwner
    {
        IPAddress getIdentity();

    }
}
