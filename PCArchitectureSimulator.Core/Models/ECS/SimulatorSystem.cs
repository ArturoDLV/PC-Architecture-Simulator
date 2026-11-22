using System.Collections.Immutable;

public static class SimulatorSystem
{
    private static uint NextID = 0;
    private static readonly Stack<uint> FreedIDs = new Stack<uint>();
    
    /// <summary>
    /// Makes a new logic device entity. Returns the ID of created entity.
    /// </summary>
    public static uint CreateLogicDevice(ComponentDelay Delay, ComponentInputs Inputs, ComponentOutputs Outputs, ComponentLogic Logic)
    {
        #region Validation: Logic device

        ArgumentNullException.ThrowIfNull(Delay);
        ArgumentNullException.ThrowIfNull(Inputs);
        ArgumentNullException.ThrowIfNull(Outputs);
        ArgumentNullException.ThrowIfNull(Logic);

        if (Delay.Picoseconds <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(Delay), "Delay must be positive");
        }

        if (Inputs.Inputs.IsDefault == true)
        {
            throw new ArgumentException(nameof(Inputs), "Inputs is uninitialized.");
        }
        foreach (Node n in Inputs.Inputs)
        {
            if (n == null)
            {
                throw new ArgumentException(nameof(Inputs), "Inputs contains at least one null Node");
            }
        }

        if (Outputs.Outputs.IsDefault == true)
        {
            throw new ArgumentException(nameof(Outputs), "Outputs is uninitialized.");
        }
        foreach (Node n in Outputs.Outputs)
        {
            if (n == null)
            {
                throw new ArgumentException(nameof(Outputs), "Outputs contains at least one null Node");
            }
        }

        if (Inputs.Inputs.Length < Logic.MinInputs || Inputs.Inputs.Length > Logic.MaxInputs)
        {
            throw new ArgumentOutOfRangeException(nameof(Inputs), $"{Logic.Name} requires {Logic.MinInputs} to {Logic.MaxInputs} inputs.");
        }

        if (Outputs.Outputs.Length != Logic.OutputCount)
        {
            throw new ArgumentOutOfRangeException(nameof(Outputs), $"{Logic.Name} requires exactly {Logic.OutputCount} output(s).");
        }

        #endregion

        #region Creation: Logic device

        uint ID;
        if (FreedIDs.Count > 0)
        {
            ID = FreedIDs.Pop();
        }
        else
        {
            ID = NextID;
            NextID++;
        }

        ComponentDictionaries.Delay.Add(ID, Delay);
        ComponentDictionaries.Logic.Add(ID, Logic);
        ComponentDictionaries.InputNodes.Add(ID, Inputs);
        ComponentDictionaries.OutputNodes.Add(ID, Outputs);
        ComponentDictionaries.OutputTransition.Add(ID, new ComponentOutputTransition(Outputs.Outputs));

        LogicState[] iBuff = new LogicState[Inputs.Inputs.Length];
        for (int i = 0; i < Inputs.Inputs.Length; i++)
        {
            iBuff[i] = Inputs.Inputs[i].State;
        }

        LogicState[] oBuff = new LogicState[Outputs.Outputs.Length];
        for (int i = 0; i < Outputs.Outputs.Length; i++)
        {
            oBuff[i] = Outputs.Outputs[i].State;
        }

        ComponentDictionaries.InputBuffer.Add(ID, iBuff);
        ComponentDictionaries.OutputBuffer.Add(ID, oBuff);

        EntityGroups.LogicDevice.Add(ID);

        return ID;

        #endregion
    }

    /// <summary>
    /// Tries to delete a logic device entity made by CreateLogicDevice()
    /// </summary>
    public static bool TryDeleteLogicDevice(uint ID)
    {
        if (EntityGroups.LogicDevice.Contains(ID) == false)
        {
            return false;
        }
        else
        {
            ComponentDictionaries.Delay.Remove(ID);
            ComponentDictionaries.Logic.Remove(ID);
            ComponentDictionaries.InputNodes.Remove(ID);
            ComponentDictionaries.InputBuffer.Remove(ID);
            ComponentDictionaries.OutputNodes.Remove(ID);
            ComponentDictionaries.OutputBuffer.Remove(ID);
            ComponentDictionaries.OutputTransition.Remove(ID);

            EntityGroups.LogicDevice.Remove(ID);

            FreedIDs.Push(ID);

            return true;
        }
    }

    /// <summary>
    /// Advances the simulation by a given amount of Picoseconds
    /// </summary>
    public static void AdvanceByTick(long Picoseconds = 1)
    {
        if (Picoseconds < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(Picoseconds), "Tick must be a non-negative number.");
        }
        
        #region Advance: Logic device

        foreach (uint ID in EntityGroups.LogicDevice)
        {
            long delay = ComponentDictionaries.Delay[ID].Picoseconds;
            ComponentOutputTransition transition = ComponentDictionaries.OutputTransition[ID];
            ImmutableArray<Node> inputs = ComponentDictionaries.InputNodes[ID].Inputs;
            LogicState[] iBuff = ComponentDictionaries.InputBuffer[ID];
            LogicState[] oBuff = ComponentDictionaries.OutputBuffer[ID];

            for (int i = 0; i < iBuff.Length; i++)
            {
                iBuff[i] = inputs[i].State;
            }

            ComponentDictionaries.Logic[ID].Mapping(iBuff, transition.Computed);

            for (int i = 0; i < oBuff.Length; i++)
            {
                if (transition.Computed[i] != transition.Pending[i])
                {
                    transition.Pending[i] = transition.Computed[i];
                    if (transition.Pending[i] == transition.Settled[i])
                    {
                        transition.TimeRemaining[i] = 0;
                    }
                    else
                    {
                        transition.TimeRemaining[i] = delay;
                    }
                }

                if (transition.TimeRemaining[i] > 0)
                {
                    transition.TimeRemaining[i] -= Picoseconds;
                }

                if (transition.TimeRemaining[i] <= 0)
                {
                    transition.TimeRemaining[i] = 0;
                    transition.Settled[i] = transition.Pending[i];
                }

                if (transition.TimeRemaining[i] > 0)
                {
                    oBuff[i] = LogicState.Undefined;
                }
                else
                {
                    oBuff[i] = transition.Settled[i];
                }
            }
        }

        foreach (uint ID in EntityGroups.LogicDevice)
        {
            ImmutableArray<Node> outputs = ComponentDictionaries.OutputNodes[ID].Outputs;
            LogicState[] oBuff = ComponentDictionaries.OutputBuffer[ID];

            for (int i = 0; i < outputs.Length; i++)
            {
                outputs[i].State = oBuff[i];
            }
        }

        #endregion
    }
}

#region Helpers

file static class ComponentDictionaries
{
    public static readonly Dictionary<uint, ComponentDelay> Delay = new Dictionary<uint, ComponentDelay>();
    public static readonly Dictionary<uint, ComponentInputs> InputNodes = new Dictionary<uint, ComponentInputs>();
    public static readonly Dictionary<uint, LogicState[]> InputBuffer = new Dictionary<uint, LogicState[]>();
    public static readonly Dictionary<uint, ComponentOutputs> OutputNodes = new Dictionary<uint, ComponentOutputs>();
    public static readonly Dictionary<uint, LogicState[]> OutputBuffer = new Dictionary<uint, LogicState[]>();
    public static readonly Dictionary<uint, ComponentOutputTransition> OutputTransition = new Dictionary<uint, ComponentOutputTransition>();
    public static readonly Dictionary<uint, ComponentLogic> Logic = new Dictionary<uint, ComponentLogic>();
}

file static class EntityGroups
{
    public static readonly HashSet<uint> LogicDevice = new HashSet<uint>();
    public static readonly HashSet<uint> Wire = new HashSet<uint>();
}

#endregion