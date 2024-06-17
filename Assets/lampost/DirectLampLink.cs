using System.Collections.Generic;

public static class DirectLampLink
{
    private static ComBlockLamp ComBlockA = new ComBlockLamp(4, 2);
    private static ComBlockLamp ComBlockB = new ComBlockLamp(4, 2);
    private static ComBlockLamp ComBlockC = new ComBlockLamp(4, 0);
    private static ComBlockLamp ComBlockD = new ComBlockLamp(4, 0);
    private static ComBlockLamp ComBlockE = new ComBlockLamp(3, 2);
    private static ComBlockLamp ComBlockF = new ComBlockLamp(4, 2);

    public static void AssignTomanager(LampostManager communicator, LaneId laneId, NodeType nodeType, int nodnum)
    {
        switch (laneId)
        {
            case LaneId.A:
                AddToComblock(ref ComBlockA, ref communicator, nodeType, nodnum);
                break;

            case LaneId.B:
                AddToComblock(ref ComBlockB, ref communicator, nodeType, nodnum);
                break;

            case LaneId.C:
                AddToComblock(ref ComBlockC, ref communicator, nodeType, nodnum);
                break;

            case LaneId.D:
                AddToComblock(ref ComBlockD, ref communicator, nodeType, nodnum);
                break;

            case LaneId.E:
                AddToComblock(ref ComBlockE, ref communicator, nodeType, nodnum);
                break;

            case LaneId.F:
                AddToComblock(ref ComBlockF, ref communicator, nodeType, nodnum);
                break;

            default:
                return;
        }
    }

    public static void AddToComblock(ref ComBlockLamp block, ref LampostManager laneCommunicator, NodeType nodeType, int lanenum)
    {
        block.AddToList(ref laneCommunicator, nodeType, lanenum);
    }

    public static void UpdateLights(recieverpakket.SignalGroup signalGroup)
    {
        ComBlockA.UpdateAllLamps(signalGroup.blocksMsg.A);
        ComBlockB.UpdateAllLamps(signalGroup.blocksMsg.B);
        ComBlockC.UpdateAllLamps(signalGroup.blocksMsg.C);
        ComBlockD.UpdateAllLamps(signalGroup.blocksMsg2.D);
        ComBlockE.UpdateAllLamps(signalGroup.blocksMsg2.E);
        ComBlockF.UpdateAllLamps(signalGroup.blocksMsg2.F);
    }

    public static void UpdatRandomlyLights()
    {
        ComBlockA.RandomizeLampStates();
        ComBlockB.RandomizeLampStates();
        ComBlockC.RandomizeLampStates();
        ComBlockD.RandomizeLampStates();
        ComBlockE.RandomizeLampStates();
        ComBlockF.RandomizeLampStates();
    }
}

public class ComBlockLamp
{
    //the max car com to acummilate
    private int maxCarCum;

    private int maxBikeCum;

    private int maxPedCum;

    //Car lanes can parse busses and prio vehicles
    private List<LampostManager> communicatorsCars = new List<LampostManager>();

    //optional add only when needed
    private List<LampostManager> communicatorsBikes = new List<LampostManager>();

    private List<LampostManager> communicatorsPeds = new List<LampostManager>();

    private List<LampostManager> Bussnumbers = new List<LampostManager>();

    public ComBlockLamp(int maxCarCum, int maxBikeCum)
    {
        this.maxCarCum = maxCarCum;
        this.maxBikeCum = maxBikeCum;
        //CreateContainerList(ref communicatorsCars, maxCarCum);
        //CreateBikeAndPedlists();
    }

    public void RandomizeLampStates()
    {
        setRangedStates(ref communicatorsCars);
        setRangedStates(ref communicatorsPeds);
        setRangedStates(ref communicatorsBikes);
    }

    private void setRangedStates(ref List<LampostManager> lamposts)
    {
        foreach (LampostManager lampost in lamposts)
        {
            lampost.SetLight(UnityEngine.Random.Range(-1, 3));
        }
    }

    public void UpdateAllLamps(recieverpakket.LightCarLanemsg lightLanemsg)
    {
        for (int i = 0; i < lightLanemsg.Cars.Count; i++)
        {
            communicatorsCars[i].SetLight(lightLanemsg.Cars[i]);
        }
    }

    public void UpdateAllLamps(recieverpakket.LightNormalLanemsg lightLanemsg)
    {
        for (int i = 0; i < lightLanemsg.Cars.Count; i++)
        {
            communicatorsCars[i].SetLight(lightLanemsg.Cars[i]);
        }
        for (int i = 0; i < lightLanemsg.bikers.Count; i++)
        {
            communicatorsBikes[i].SetLight(lightLanemsg.bikers[i]);
        }
        for (int i = 0; i < lightLanemsg.Walkers.Count; i++)
        {
            communicatorsPeds[i].SetLight(lightLanemsg.Walkers[i]);
        }
    }

    public void UpdateAllLamps(recieverpakket.LightFullLanemsg lightLanemsg)
    {
        for (int i = 0; i < lightLanemsg.Cars.Count; i++)
        {
            communicatorsCars[i].SetLight(lightLanemsg.Cars[i]);
        }
        for (int i = 0; i < lightLanemsg.bikers.Count; i++)
        {
            communicatorsBikes[i].SetLight(lightLanemsg.bikers[i]);
        }
        for (int i = 0; i < lightLanemsg.Walkers.Count; i++)
        {
            communicatorsPeds[i].SetLight(lightLanemsg.Walkers[i]);
        }
        for (int i = 0; i < lightLanemsg.busje.Count; i++)
        {
            Bussnumbers[i].SetLight(lightLanemsg.busje[i]);
        }
    }

    private void CreateBikeAndPedlists()
    {
        if (maxBikeCum > 0)
        {
            maxPedCum = maxBikeCum * 2;
            CreateContainerList(ref communicatorsBikes, maxBikeCum);
            CreateContainerList(ref communicatorsPeds, maxPedCum);
        }
    }

    private void CreateContainerList(ref List<LampostManager> communicators, int maxAmount)
    {
        communicators = new List<LampostManager>();
        for (int i = 0; i < maxAmount; i++)
        {
            communicators.Add(null);
        }
    }

    public void AddToList(ref LampostManager laneCommunicator, NodeType type, int number)
    {
        switch (type)
        {
            case NodeType.Pedestrian:

                communicatorsPeds.Add(laneCommunicator);
                //communicatorsPeds[laneCommunicator.GetComlaneNr()] = laneCommunicator;
                break;

            case NodeType.Biker:

                communicatorsBikes.Add(laneCommunicator);
                //communicatorsBikes[laneCommunicator.GetComlaneNr()] = laneCommunicator;
                break;

            case NodeType.HighSpeed:

                communicatorsCars.Add(laneCommunicator);
                //communicatorsCars[laneCommunicator.GetComlaneNr()] = laneCommunicator;
                break;

            default:
                break;
        }
    }
}