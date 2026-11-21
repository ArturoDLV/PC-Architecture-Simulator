using System.Collections.Immutable;

public class ComponentLogic
{
    private readonly ImmutableArray<Node> Inputs;
    private readonly ImmutableArray<Node> Outputs;
    private readonly LogicDefinition Logic;

    private readonly LogicState[] InputBuffer;
    private readonly LogicState[] OutputBuffer;

    public ComponentLogic(Node[] Inputs, Node[] Outputs, LogicDefinition Logic)
    {
        ArgumentNullException.ThrowIfNull(Inputs, nameof(Inputs));
        ArgumentNullException.ThrowIfNull(Outputs, nameof(Outputs));
        ArgumentNullException.ThrowIfNull(Logic, nameof(Logic));

        ImmutableArray<Node> inputsCopy = ImmutableArray.Create(Inputs);
        ImmutableArray<Node> outputsCopy = ImmutableArray.Create(Outputs);

        if (inputsCopy.Length == 0 || outputsCopy.Length == 0)
        {
            throw new ArgumentException("Inputs and Outputs cannot be empty.");
        }

        if (inputsCopy.Length < Logic.MinInputs || inputsCopy.Length > Logic.MaxInputs)
        {
            throw new ArgumentException($"{Logic.Name} requires {Logic.MinInputs} to {Logic.MaxInputs} inputs. Got {inputsCopy.Length}.", nameof(Inputs));
        }

        if (outputsCopy.Length != Logic.OutputCount)
        {
            throw new ArgumentException($"{Logic.Name} requires exactly {Logic.OutputCount} outputs. Got {outputsCopy.Length}.", nameof(Outputs));
        }

        this.Inputs = inputsCopy;
        this.Outputs = outputsCopy;
        this.Logic = Logic;

        InputBuffer = new LogicState[Inputs.Length];
        OutputBuffer = new LogicState[Outputs.Length];
    }

    public void Update()
    {
        for (int i = 0; i < Inputs.Length; i++)
        {
            InputBuffer[i] = Inputs[i].State;
        }

        Logic.Mapping(InputBuffer, OutputBuffer);

        for (int i = 0; i < Outputs.Length; i++)
        {
            Outputs[i].State = OutputBuffer[i];
        }
    }
}