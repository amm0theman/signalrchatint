using SignalRChat.Value_Objects;
using System;

namespace SignalRChat.Entities
{
    class Parcel
    {
        private IOwner Owner { get; }           //Creator/owner of the message
        private DateTime TimeStamp { get; }    //Time of message creation
        private Message message { get; }        //Message contained in parcel
        
        public Parcel(Message message, IOwner Owner)
        {
            this.Owner = Owner;
            this.message = message;
            TimeStamp = DateTime.Now;
        }
    }
}
