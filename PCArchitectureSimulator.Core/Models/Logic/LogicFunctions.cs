public static class LogicFunctions
{
    public static readonly ComponentLogic AND = new ComponentLogic("AND", 2, int.MaxValue, 1, ANDMapping);
    private static void ANDMapping(ReadOnlySpan<LogicState> Inputs, Span<LogicState> Outputs)
    {
        bool hasUndefined = false;

        foreach (LogicState input in Inputs)
        {
            if (input == LogicState.Low)
            {
                Outputs[0] = LogicState.Low;
                return;
            }
            
            if (input == LogicState.Undefined || input == LogicState.HighImpedance)
            {
                hasUndefined = true;
            }
        }

        Outputs[0] = hasUndefined ? LogicState.Undefined : LogicState.High;
        return;
    }

    public static readonly ComponentLogic OR = new ComponentLogic("OR", 2, int.MaxValue, 1, ORMapping);
    private static void ORMapping(ReadOnlySpan<LogicState> Inputs, Span<LogicState> Outputs)
    {
        bool hasUndefined = false;

        foreach (LogicState input in Inputs)
        {
            if (input == LogicState.High)
            {
                Outputs[0] = LogicState.High;
                return;
            }
            
            if (input == LogicState.Undefined || input == LogicState.HighImpedance)
            {
                hasUndefined = true;
            }
        }

        Outputs[0] = hasUndefined ? LogicState.Undefined : LogicState.Low;
        return;
    }

    public static readonly ComponentLogic NAND = new ComponentLogic("NAND", 2, int.MaxValue, 1, NANDMapping);
    private static void NANDMapping(ReadOnlySpan<LogicState> Inputs, Span<LogicState> Outputs)
    {
        bool hasUndefined = false;

        foreach (LogicState input in Inputs)
        {
            if (input == LogicState.Low)
            {
                Outputs[0] = LogicState.High;
                return;
            }
            
            if (input == LogicState.Undefined || input == LogicState.HighImpedance)
            {
                hasUndefined = true;
            }
        }

        Outputs[0] = hasUndefined ? LogicState.Undefined : LogicState.Low;
        return;
    }

    public static readonly ComponentLogic NOR = new ComponentLogic("NOR", 2, int.MaxValue, 1, NORMapping);
    private static void NORMapping(ReadOnlySpan<LogicState> Inputs, Span<LogicState> Outputs)
    {
        bool hasUndefined = false;

        foreach (LogicState input in Inputs)
        {
            if (input == LogicState.High)
            {
                Outputs[0] = LogicState.Low;
                return;
            }
            
            if (input == LogicState.Undefined || input == LogicState.HighImpedance)
            {
                hasUndefined = true;
            }
        }

        Outputs[0] = hasUndefined ? LogicState.Undefined : LogicState.High;
        return;
    }

    public static readonly ComponentLogic NOT = new ComponentLogic("NOT", 1, 1, 1, NOTMapping);
    private static void NOTMapping(ReadOnlySpan<LogicState> Inputs, Span<LogicState> Outputs)
    {
        LogicState input = Inputs[0];
        
        switch (input)
        {
            case LogicState.Low:
            {
                Outputs[0] = LogicState.High;
                return;
            }
            case LogicState.High:
            {
                Outputs[0] = LogicState.Low;
                return;
            }
            case LogicState.Undefined:
            {
                Outputs[0] = LogicState.Undefined;
                return;
            }
            case LogicState.HighImpedance:
            {
                Outputs[0] = LogicState.Undefined;
                return;
            }
            default:
            {
                // Should never trigger
                throw new ArgumentOutOfRangeException(nameof(Inputs), "Invalid logic state");
            }
        }
    }
}