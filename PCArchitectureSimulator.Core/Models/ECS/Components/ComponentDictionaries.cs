internal static class ComponentDictionaries
{
    public static readonly Dictionary<uint, ComponentDelay> Delay = new Dictionary<uint, ComponentDelay>();
    public static readonly Dictionary<uint, ComponentInputs> InputNodes = new Dictionary<uint, ComponentInputs>();
    public static readonly Dictionary<uint, LogicState[]> InputBuffer = new Dictionary<uint, LogicState[]>();
    public static readonly Dictionary<uint, ComponentSignalQueue.Q> SignalQueues =  new Dictionary<uint, ComponentSignalQueue.Q>();
    public static readonly Dictionary<uint, LogicState[]> ReadySignals = new Dictionary<uint, LogicState[]>();
    public static readonly Dictionary<uint, ComponentOutputs> OutputNodes = new Dictionary<uint, ComponentOutputs>();
    public static readonly Dictionary<uint, LogicState[]> OutputBuffer = new Dictionary<uint, LogicState[]>();
    public static readonly Dictionary<uint, ComponentLogic> Logic = new Dictionary<uint, ComponentLogic>();
}