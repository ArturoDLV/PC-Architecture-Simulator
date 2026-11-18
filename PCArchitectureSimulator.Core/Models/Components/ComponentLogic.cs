public class ComponentLogic
{
    private readonly Node[] Inputs;
    private readonly Node[] Outputs;
    private readonly Func<LogicState[], LogicState[]> MappingFunction;

    public ComponentLogic(Node[] Inputs, Node[] Outputs, Func<LogicState[], LogicState[]> MappingFunction)
    {
        int inputCount = Inputs.Length;
        int outputCount = Outputs.Length;

        if (inputCount == 0 || outputCount == 0)
        {
            throw new ArgumentException("Inputs and Outputs cannot be empty.");
        }

        LogicState[] testInput = new LogicState[Inputs.Length];
        LogicState[] testOutput = MappingFunction(testInput);
        if (testOutput == null || testOutput.Length != Outputs.Length)
        {
            throw new ArgumentException("MappingFunction returns an incorrect number of outputs.");
        }

        this.Inputs = Inputs;
        this.Outputs = Outputs;
        this.MappingFunction = MappingFunction;
    }

    public void Update()
    {
        int inputCount = Inputs.Length;
        int outputCount = Outputs.Length;
        LogicState[] inputStates = new LogicState[inputCount];
        LogicState[] outputStates = new LogicState[outputCount];

        for (int i = 0; i < inputCount; i++)
        {
            inputStates[i] = Inputs[i].State;
        }

        outputStates = MappingFunction(inputStates);

        for (int i = 0; i < outputCount; i++)
        {
            Outputs[i].State = outputStates[i];
        }
    }
}