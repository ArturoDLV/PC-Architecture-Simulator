using System.Collections.Immutable;
using System.Runtime.CompilerServices;

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
            throw new ArgumentOutOfRangeException(nameof(Picoseconds), "Tick must be a non-negative integer.");
        }

        AdvanceWires(Picoseconds);
        AdvanceLogicEntities(Picoseconds);

        SimulationTime += (ulong)Picoseconds;
    }

    private static void AdvanceWires(long Picoseconds)
    {
        foreach (uint ID in EntityGroups.Wire)
        {
            Queue<ComponentSignalQueue.Signal> queue = ComponentDictionaries.SignalQueues[ID].SignalQueue[0];
            Node input = ComponentDictionaries.InputNodes[ID].Inputs[0];
            LogicState[] iBuff = ComponentDictionaries.InputBuffer[ID];

            if (iBuff[0] != input.State)
            {
                iBuff[0] = input.State;

                ComponentSignalQueue.Signal signal = new ComponentSignalQueue.Signal(iBuff[0], ComponentDictionaries.Delay[ID].Picoseconds);
                queue.Enqueue(signal);
            }

            LogicState? readyState = AdvanceSignalQueue(queue, Picoseconds);
            if (readyState != null)
            {
                ComponentDictionaries.OutputBuffer[ID][0] = readyState.Value;
            }
        }

        foreach (uint ID in EntityGroups.Wire)
        {
            Node output = ComponentDictionaries.OutputNodes[ID].Outputs[0];
            LogicState[] oBuff = ComponentDictionaries.OutputBuffer[ID];

            output.State = oBuff[0];
        }
    }

    private static void AdvanceLogicEntities(long Picoseconds)
    {
        foreach (uint ID in EntityGroups.LogicDevice)
        {
            ImmutableArray<Queue<ComponentSignalQueue.Signal>> queues = ComponentDictionaries.SignalQueues[ID].SignalQueue;
            ImmutableArray<Node> inputs = ComponentDictionaries.InputNodes[ID].Inputs;
            LogicState[] iBuff = ComponentDictionaries.InputBuffer[ID];
            LogicState[] readySignals = ComponentDictionaries.ReadySignals[ID];

            for (int i = 0; i < inputs.Length; i++)
            {
                Queue<ComponentSignalQueue.Signal> currentQ = queues[i];

                if (iBuff[i] != inputs[i].State)
                {
                    iBuff[i] = inputs[i].State;

                    ComponentSignalQueue.Signal signal = new ComponentSignalQueue.Signal(iBuff[i], ComponentDictionaries.Delay[ID].Picoseconds);
                    currentQ.Enqueue(signal);
                }

                LogicState? readyState = AdvanceSignalQueue(currentQ, Picoseconds);
                if (readyState != null)
                {
                    readySignals[i] = readyState.Value;
                }
            }

            ComponentDictionaries.Logic[ID].Mapping(readySignals, ComponentDictionaries.OutputBuffer[ID]);
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

    private static LogicState? AdvanceSignalQueue(Queue<ComponentSignalQueue.Signal> SignalQueue, long Picoseconds)
    {
        LogicState? readyState = null;

        while (SignalQueue.Count > 0)
        {
            ComponentSignalQueue.Signal signal = SignalQueue.Peek();
            signal.TimeRemaining -= Picoseconds;

            if (signal.TimeRemaining > 0)
            {
                break;
            }

            SignalQueue.Dequeue();
            readyState = signal.State;

            Picoseconds = -signal.TimeRemaining;
        }

        return readyState;
    }
}