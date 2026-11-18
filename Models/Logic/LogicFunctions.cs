public static class LogicFunctions
{
    public static LogicState[] AND(LogicState[] Inputs)
    {
        bool hasUndefined = false;

        foreach (LogicState input in Inputs)
        {
            if (input == LogicState.Low)
            {
                return [LogicState.Low];
            }
            
            if (input == LogicState.Undefined || input == LogicState.HighImpedance)
            {
                hasUndefined = true;
            }
        }

        return [hasUndefined ? LogicState.Undefined : LogicState.High];
    }

    public static LogicState[] OR(LogicState[] Inputs)
    {
        bool hasUndefined = false;

        foreach (LogicState input in Inputs)
        {
            if (input == LogicState.High)
            {
                return [LogicState.High];
            }
            
            if (input == LogicState.Undefined || input == LogicState.HighImpedance)
            {
                hasUndefined = true;
            }
        }

        return [hasUndefined ? LogicState.Undefined : LogicState.Low];
    }

    public static LogicState[] NOT(LogicState[] Inputs)
    {
        if (Inputs.Length != 1)
        {
            throw new ArgumentException("NOT function requires exactly one input.");
        }

        LogicState input = Inputs[0];
        switch (input)
        {
            case LogicState.Low:
            {
                return [LogicState.High];
            }
            case LogicState.High:
            {
                return [LogicState.Low];
            }
            case LogicState.Undefined:
            {
                return [LogicState.Undefined];
            }
            case LogicState.HighImpedance:
            {
                return [LogicState.HighImpedance];
            }
            default:
            {
                throw new ArgumentOutOfRangeException();
            }
        }
    }
}