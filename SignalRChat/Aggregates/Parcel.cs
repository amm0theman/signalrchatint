using SignalRChat.Value_Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace SignalRChat.Entities
{
    class Parcel
    {
        private IOwner Owner { get; }           //Creator/owner of the message
        private TimeStamp TimeStamp { get; }    //Time of message creation
        private Message message { get; }        //Message contained in parcel
    }
}
