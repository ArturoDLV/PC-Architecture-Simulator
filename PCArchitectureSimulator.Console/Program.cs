// RS Latch

Node R = new Node(LogicState.Low);
Node S = new Node(LogicState.High);
Node Q = new Node(LogicState.Undefined);
Node Qn = new Node(LogicState.Undefined);


ComponentDelay delay = new ComponentDelay(1);
ComponentLogic nor1 = new ComponentLogic([R, Qn], [Q], LogicFunctions.NOR);
ComponentLogic nor2 = new ComponentLogic([S, Q], [Qn], LogicFunctions.NOR);

SimulatorSystem.CreateEntity(delay, nor1);
SimulatorSystem.CreateEntity(delay, nor2);

Console.WriteLine("\nInitial state");
Console.WriteLine("Node R: " + R.State.ToString());
Console.WriteLine("Node S: " + S.State.ToString());
Console.WriteLine("Node Q: " + Q.State.ToString());
Console.WriteLine("Node Qn: " + Qn.State.ToString());

SimulatorSystem.Tick(1);
Console.WriteLine("\nTick 1");
Console.WriteLine("Node R: " + R.State.ToString());
Console.WriteLine("Node S: " + S.State.ToString());
Console.WriteLine("Node Q: " + Q.State.ToString());
Console.WriteLine("Node Qn: " + Qn.State.ToString());

SimulatorSystem.Tick(1);
Console.WriteLine("\nTick 1");
Console.WriteLine("\nNode R: " + R.State.ToString());
Console.WriteLine("Node S: " + S.State.ToString());
Console.WriteLine("Node Q: " + Q.State.ToString());
Console.WriteLine("Node Qn: " + Qn.State.ToString());

S.State = LogicState.Low;
SimulatorSystem.Tick(1);
Console.WriteLine("\nS to low, then tick 1");
Console.WriteLine("Node R: " + R.State.ToString());
Console.WriteLine("Node S: " + S.State.ToString());
Console.WriteLine("Node Q: " + Q.State.ToString());
Console.WriteLine("Node Qn: " + Qn.State.ToString());

SimulatorSystem.Tick(1);
Console.WriteLine("\nTick 1");
Console.WriteLine("Node R: " + R.State.ToString());
Console.WriteLine("Node S: " + S.State.ToString());
Console.WriteLine("Node Q: " + Q.State.ToString());
Console.WriteLine("Node Qn: " + Qn.State.ToString());

SimulatorSystem.Tick(1);
Console.WriteLine("\nTick 1");
Console.WriteLine("Node R: " + R.State.ToString());
Console.WriteLine("Node S: " + S.State.ToString());
Console.WriteLine("Node Q: " + Q.State.ToString());
Console.WriteLine("Node Qn: " + Qn.State.ToString());

Console.ReadLine();