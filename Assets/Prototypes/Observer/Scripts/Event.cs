using System.Collections.Generic;

namespace ObserverPattern
{
    public class Event
    {
        public EventName eventName;
        public Dictionary<string, object> payload;

        public Event(EventName eventName)
        {
            this.eventName = eventName;
            this.payload = new Dictionary<string, object>();
        }
    }
}