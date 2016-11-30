public class DoorOpenTrigger : AkTriggerBase
{
    public void Open()
    {
        if (triggerDelegate != null)
        {
            triggerDelegate(null);
        }
    }
}
