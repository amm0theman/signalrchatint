using SignalRChat.Value_Objects;
using SignalRChat.Entities;
using System;

namespace SignalRChat.Aggregates
{
    public class Parcel
    {
        public User Owner { get; }           //Creator/owner of the message
        public DateTime TimeStamp { get; }     //Time of message creation
        public Message message { get; }        //Message contained in parcel
        
        public Parcel(Message message, User Owner)
        {
            this.Owner = Owner;
            this.message = message;
            TimeStamp = DateTime.Now;
        }
    }
}
