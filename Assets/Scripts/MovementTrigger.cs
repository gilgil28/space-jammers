public class MovementTrigger :  AkTriggerBase
{
    private bool _triggered;

    public void Trigger()
    {
        if (_triggered)
        {
            return;
        }
        triggerDelegate?.Invoke(null);
        _triggered = true;
    }

    public void Stop()
    {
        GetComponent<AkAmbient>().Stop(0);
        _triggered = false;
    }
}