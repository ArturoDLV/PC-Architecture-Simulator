using System.Collections.Immutable;

Node R = new Node(LogicState.Low);
Node S = new Node(LogicState.Low);
Node Q = new Node(LogicState.Low);
Node Qn = new Node(LogicState.High);

ComponentDelay delay = new ComponentDelay(1);
ComponentLogic nor = LogicFunctions.NOR;

EntityManager.CreateLogicDevice(delay, new ComponentInputs(ImmutableArray.Create([R, Qn])), new ComponentOutputs(ImmutableArray.Create([Q])), nor);
EntityManager.CreateLogicDevice(delay, new ComponentInputs(ImmutableArray.Create([S, Q])), new ComponentOutputs(ImmutableArray.Create([Qn])), nor);

Console.WriteLine();

PrintState("Initial state");
SimulatorSystem.AdvanceByTick(1);
S.State = LogicState.High;
PrintState("Tick: 1; High S");
SimulatorSystem.AdvanceByTick(1);
PrintState("Tick: 2");
SimulatorSystem.AdvanceByTick(1);
S.State = LogicState.Low;
PrintState("Tick: 3; Low S");
SimulatorSystem.AdvanceByTick(1);
PrintState("Tick: 4");

SetWhite();
Console.WriteLine();
Console.ReadLine();

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
    }
}