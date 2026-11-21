using System;

public static class SimulatorSystem
{
    private static int NextID = 0;

    private static ComponentDelay[] DelayArray = [];
    private static ComponentLogic[] LogicArray = [];
    
    public static void CreateEntity(ComponentDelay Delay, ComponentLogic Logic)
    {
        int ID = NextID;
        NextID++;

        Array.Resize<ComponentDelay>(ref DelayArray, (ID + 1));
        Array.Resize<ComponentLogic>(ref LogicArray, (ID + 1));

        DelayArray[ID] = Delay;
        LogicArray[ID] = Logic;

        return;
    }

    public static void Tick(float Nanoseconds)
    {
        for(int i = 0; i < DelayArray.Length; i++)
        {
            if (DelayArray[i].TryTrigger(Nanoseconds) == true)
            {
                LogicArray[i].Update();
            }
        }
    }
}