using System.Collections.Immutable;

public static class ComponentSignalQueue
{
    public record Q(ImmutableArray<Queue<Signal>> SignalQueue);
    public class Signal(LogicState State, long TimeRemaining)
    {
        public readonly LogicState State = State;
        public long TimeRemaining = TimeRemaining;
    }
}