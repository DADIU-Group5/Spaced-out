using System.Collections.Generic;

public class ObserverEvent
{
    public EventName eventName;
    public Dictionary<string, object> payload;

    public ObserverEvent(EventName eventName)
    {
        this.eventName = eventName;
        this.payload = new Dictionary<string, object>();
    }
}
