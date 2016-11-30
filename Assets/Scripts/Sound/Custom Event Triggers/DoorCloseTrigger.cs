public class DoorCloseTrigger : AkTriggerBase
{
    public void Close()
    {
        if (triggerDelegate != null)
        {
            triggerDelegate(null);
        }
    }
}
