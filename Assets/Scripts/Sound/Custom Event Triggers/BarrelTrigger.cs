public class BarrelTrigger : AkTriggerBase
{
    public void TriggerBarrel()
    {
        if (triggerDelegate != null)
        {
            triggerDelegate(null);
        }
    }
}
