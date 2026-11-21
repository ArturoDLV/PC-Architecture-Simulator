public class ComponentDelay(float DelayNanoseconds)
{
    private readonly float Delay = DelayNanoseconds;
    private float ElapsedTime = 0;

    public bool TryTrigger(float TickTime)
    {
        ElapsedTime += TickTime;

        if (ElapsedTime >= Delay)
        {
            ElapsedTime = 0;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Reset()
    {
        ElapsedTime = 0;
    }
}