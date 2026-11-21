public sealed class LogicDefinition
{
    public readonly string Name;
    public readonly int MinInputs;
    public readonly int MaxInputs;
    public readonly int OutputCount;
    public readonly LogicMapping Mapping;

    public LogicDefinition(string Name, int MinInputs, int MaxInputs, int OutputCount, LogicMapping Mapping)
    {
        ArgumentNullException.ThrowIfNull(Mapping, nameof(Mapping));

        if (string.IsNullOrWhiteSpace(Name))
        {
            throw new ArgumentException("Name cannot be empty.", nameof(Name));
        }

        if (MinInputs < 1 || MaxInputs < MinInputs)
        {
            throw new ArgumentOutOfRangeException(nameof(MinInputs), "Input range is invalid.");
        }

        if (OutputCount < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(OutputCount), "Must have at least one output.");
        }

        this.Name = Name;
        this.MinInputs = MinInputs;
        this.MaxInputs = MaxInputs;
        this.OutputCount = OutputCount;
        this.Mapping = Mapping;
    }
}