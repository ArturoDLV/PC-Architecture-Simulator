public sealed class Node(LogicState InitialState = LogicState.HighImpedance)
{
    public LogicState State = InitialState;
}