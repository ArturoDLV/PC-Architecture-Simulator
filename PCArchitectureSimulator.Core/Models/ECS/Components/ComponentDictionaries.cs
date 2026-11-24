internal static class ComponentDictionaries
{
    public static readonly Dictionary<uint, ComponentDelay> Delay = new Dictionary<uint, ComponentDelay>();
    public static readonly Dictionary<uint, ComponentInputs> InputNodes = new Dictionary<uint, ComponentInputs>();
    public static readonly Dictionary<uint, LogicState[]> InputBuffer = new Dictionary<uint, LogicState[]>();
    public static readonly Dictionary<uint, ComponentOutputs> OutputNodes = new Dictionary<uint, ComponentOutputs>();
    public static readonly Dictionary<uint, LogicState[]> OutputBuffer = new Dictionary<uint, LogicState[]>();
    public static readonly Dictionary<uint, ComponentOutputTransition> OutputTransition = new Dictionary<uint, ComponentOutputTransition>();
    public static readonly Dictionary<uint, ComponentLogic> Logic = new Dictionary<uint, ComponentLogic>();
    public static readonly Dictionary<uint, ComponentSignalQueue> SignalQueue = new Dictionary<uint, ComponentSignalQueue>();
}