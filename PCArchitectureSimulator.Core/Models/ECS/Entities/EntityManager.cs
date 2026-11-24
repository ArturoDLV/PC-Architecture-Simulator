public static class EntityManager
{
    private static uint NextID = 0;
    private static readonly Stack<uint> FreedIDs = new Stack<uint>();

    #region Logic Device

    /// <summary>
    /// Makes a new logic device entity. Returns the ID of created entity.
    /// </summary>
    public static uint CreateLogicDevice(ComponentDelay Delay, ComponentInputs Inputs, ComponentOutputs Outputs, ComponentLogic Logic)
    {
        ArgumentNullException.ThrowIfNull(Delay);
        ArgumentNullException.ThrowIfNull(Inputs);
        ArgumentNullException.ThrowIfNull(Outputs);
        ArgumentNullException.ThrowIfNull(Logic);

        if (Delay.Picoseconds <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(Delay), "Delay must be positive.");
        }

        if (Inputs.Inputs.IsDefault == true)
        {
            throw new ArgumentException(nameof(Inputs), "Inputs is uninitialized.");
        }
        foreach (Node n in Inputs.Inputs)
        {
            if (n == null)
            {
                throw new ArgumentException(nameof(Inputs), "Inputs contains at least one null Node.");
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
                throw new ArgumentException(nameof(Outputs), "Outputs contains at least one null Node.");
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

    #endregion

    #region Wire

    /// <summary>
    /// Makes a new wire entity and returns the ID of created entity.
    /// Length is in millimeters, and propagation rate is in picoseconds per millimeter with a default value of 7.5
    /// which corresponds to the worst case of a standard FR-4 PCB trace.
    /// </summary>
    public static uint CreateWire(double Length, ComponentInputs Input, ComponentOutputs Output, double PropagationRate = 7.5)
    {
        ArgumentNullException.ThrowIfNull(Input);
        ArgumentNullException.ThrowIfNull(Output);

        if (Length <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(Length), "Length must be positive.");
        }

        if (PropagationRate <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(PropagationRate), "Propagation rate must be positive.");
        }

        double wireDelay = (Length * PropagationRate);
        if (wireDelay <= 0.5)
        {
            throw new ArgumentException(String.Format("Calculated wire delay is {0} picoseconds, which would round to 0 as the simulation works in discrete unit picosecond ticks.\nPlease provide a different length or propagation rate, or connect to a node directly.", wireDelay));
        }

        if (Input.Inputs.IsDefault == true)
        {
            throw new ArgumentException(nameof(Input), "Input is uninitialized.");
        }
        if (Input.Inputs.Length != 1)
        {
            throw new ArgumentException(nameof(Input), "Input must have exactly one component node.");
        }

        if (Output.Outputs.IsDefault == true)
        {
            throw new ArgumentException(nameof(Output), "Output is uninitialized.");
        }
        if (Output.Outputs.Length != 1)
        {
            throw new ArgumentException(nameof(Output), "Output must have exactly one component node.");
        }

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

        ComponentDictionaries.Delay.Add(ID, new ComponentDelay((long)Math.Round(wireDelay)));
        ComponentDictionaries.SignalQueue.Add(ID, new ComponentSignalQueue());
        ComponentDictionaries.InputNodes.Add(ID, Input);
        ComponentDictionaries.OutputNodes.Add(ID, Output);

        LogicState[] iBuff = [Input.Inputs[0].State];
        LogicState[] oBuff = [Output.Outputs[0].State];
        ComponentDictionaries.InputBuffer.Add(ID, iBuff);
        ComponentDictionaries.OutputBuffer.Add(ID, oBuff);

        EntityGroups.Wire.Add(ID);

        return ID;
    }

    /// <summary>
    /// Tries to delete a wire entity made by CreateWire()
    /// </summary>
    public static bool TryDeleteWire(uint ID)
    {
        if (EntityGroups.Wire.Contains(ID) == false)
        {
            return false;
        }
        else
        {
            ComponentDictionaries.Delay.Remove(ID);
            ComponentDictionaries.SignalQueue.Remove(ID);
            ComponentDictionaries.InputNodes.Remove(ID);
            ComponentDictionaries.InputBuffer.Remove(ID);
            ComponentDictionaries.OutputNodes.Remove(ID);
            ComponentDictionaries.OutputBuffer.Remove(ID);

            EntityGroups.Wire.Remove(ID);

            FreedIDs.Push(ID);

            return true;
        }
    }

    #endregion
}