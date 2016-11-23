public class DoorOpenCloseTrigger : AkTriggerBase
{
    public void Open()
    {
        if (triggerDelegate != null)
        {
            triggerDelegate(null);
        }
    }
}
