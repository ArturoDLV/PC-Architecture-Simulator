public static class LogicFunctions
{
    public static readonly LogicDefinition AND = new LogicDefinition("AND", 2, int.MaxValue, 1, ANDMapping);
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

    public static readonly LogicDefinition OR = new LogicDefinition("OR", 2, int.MaxValue, 1, ORMapping);
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

    public static readonly LogicDefinition NOR = new LogicDefinition("NOR", 2, int.MaxValue, 1, NORMapping);
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

    public static readonly LogicDefinition NOT = new LogicDefinition("NOT", 1, 1, 1, NOTMapping);
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
                Outputs[0] = LogicState.HighImpedance;
                return;
            }
            default:
            {
                throw new ArgumentOutOfRangeException();
            }
        }
    }
}