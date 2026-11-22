using System.Collections.Immutable;

Node R = new Node(LogicState.Low);
Node S = new Node(LogicState.High);
Node Q = new Node(LogicState.Undefined);
Node Qn = new Node(LogicState.Undefined);

ComponentDelay delay = new ComponentDelay(1);
ComponentInputs inputs1 = new ComponentInputs(ImmutableArray.Create([R, Qn]));
ComponentOutputs outputs1 = new ComponentOutputs(ImmutableArray.Create([Q]));
ComponentInputs inputs2 = new ComponentInputs(ImmutableArray.Create([S, Q]));
ComponentOutputs outputs2 = new ComponentOutputs(ImmutableArray.Create([Qn]));

SimulatorSystem.CreateLogicDevice(delay, inputs1, outputs1, LogicFunctions.NOR);
SimulatorSystem.CreateLogicDevice(delay, inputs2, outputs2, LogicFunctions.NOR);

Console.WriteLine();

PrintState("Initial state");
SimulatorSystem.AdvanceByTick(1);
PrintState("Tick: 1");
SimulatorSystem.AdvanceByTick(1);
PrintState("Tick: 2");
SimulatorSystem.AdvanceByTick(1);
S.State = LogicState.Low;
PrintState("Tick: 3; Low S");
SimulatorSystem.AdvanceByTick(1);
PrintState("Tick: 4");

Console.WriteLine();

void PrintState(string State)
{
    SetWhite();
    Console.WriteLine("\n--- " + State + " ---");

    SetWhite();
    Console.Write("R: ");
    SetLogicColor(R.State);
    Console.Write(R.State.ToString() + "\n");

    SetWhite();
    Console.Write("S: ");
    SetLogicColor(S.State);
    Console.Write(S.State.ToString() + "\n");

    SetWhite();
    Console.Write("Q: ");
    SetLogicColor(Q.State);
    Console.Write(Q.State.ToString() + "\n");

    SetWhite();
    Console.Write("Qn: ");
    SetLogicColor(Qn.State);
    Console.Write(Qn.State.ToString() + "\n");
}

void SetWhite()
{
    Console.ForegroundColor = ConsoleColor.White;
}

void SetLogicColor(LogicState LogicValue)
{
    switch(LogicValue)
    {
        case LogicState.Low:
        {
            Console.ForegroundColor = ConsoleColor.Red;
            break;
        }
        case LogicState.High:
        {
            Console.ForegroundColor = ConsoleColor.Green;
            break;
        }
        case LogicState.HighImpedance:
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            break;
        }
        case LogicState.Undefined:
        {
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            break;
        }
    }
}