using System.Collections.Immutable;

public sealed class ComponentOutputTransition
{
    public readonly LogicState[] Computed;
    public readonly LogicState[] Pending;
    public readonly LogicState[] Settled;
    public readonly long[] TimeRemaining;

    public ComponentOutputTransition(ImmutableArray<Node> Outputs)
    {
        if (Outputs.IsDefault == true)
        {
            throw new ArgumentException("Outputs must be initialized.", nameof(Outputs));
        }

        int count = Outputs.Length;
        Computed = new LogicState[count];
        Pending = new LogicState[count];
        Settled = new LogicState[count];
        TimeRemaining = new long[count];
        Array.Fill(TimeRemaining, 0);

        for (int i = 0; i < count; i++)
        {
            Computed[i] = Pending[i] = Settled[i] = Outputs[i].State;
        }
    }
}