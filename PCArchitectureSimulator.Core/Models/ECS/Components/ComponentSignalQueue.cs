public sealed class ComponentSignalQueue
{
    public readonly Queue<Signal> SignalQueue = new Queue<Signal>();
    public class Signal(LogicState State, long RemainingTime)
    {
        public LogicState State = State;
        public long RemainingTime = RemainingTime;
    }
}