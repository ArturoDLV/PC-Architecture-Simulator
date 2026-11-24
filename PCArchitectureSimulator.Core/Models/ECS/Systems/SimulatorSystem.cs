using System.Collections.Immutable;

public static class SimulatorSystem
{
    private static ulong SimulationTime = 0;

    /// <summary>
    /// Returns the elapsed simulated picoseconds since the simulation started.
    /// </summary>
    public static ulong GetSimulationTime()
    {
        return SimulationTime;
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

        //AdvanceWires(Picoseconds);
        AdvanceLogicEntities(Picoseconds);

        SimulationTime += (ulong)Picoseconds;
    }

    private static void AdvanceWires(long Picoseconds)
    {
        foreach (uint ID in EntityGroups.Wire)
        {
            Queue<ComponentSignalQueue.Signal> signalQueue = ComponentDictionaries.SignalQueue[ID].SignalQueue;
            Node inputNode = ComponentDictionaries.InputNodes[ID].Inputs[0];
            LogicState[] iBuff = ComponentDictionaries.InputBuffer[ID];

            if (iBuff[0] != inputNode.State)
            {
                iBuff[0] = inputNode.State;
                long delay = ComponentDictionaries.Delay[ID].Picoseconds;
                signalQueue.Enqueue(new ComponentSignalQueue.Signal(iBuff[0], delay));
            }

            if (signalQueue.Count == 0)
            {
                continue;
            }
            else
            {
                ComponentSignalQueue.Signal next = signalQueue.First();
                
                next.RemainingTime -= Picoseconds;
                if (next.RemainingTime <= 0)
                {
                    LogicState[] oBuff = ComponentDictionaries.OutputBuffer[ID];

                    next = signalQueue.Dequeue();
                    oBuff[0] = next.State;
                }
            }
        }
    }

    private static void AdvanceLogicEntities(long Picoseconds)
    {
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
    }
}