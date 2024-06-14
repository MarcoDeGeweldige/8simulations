using System.Collections.Generic;

using UnityEngine;


public enum MessageType
{
    normalmsg,
    normalmsgBus,
    CarOnlymsg
}
public static class SensorDatamanager
{

    private static ComBlock ComBlockA = new ComBlock(4,2, MessageType.normalmsg);
    private static ComBlock ComBlockB = new ComBlock(4,2, MessageType.normalmsgBus);
    private static ComBlock ComBlockC = new ComBlock(4,0, MessageType.CarOnlymsg);
    private static ComBlock ComBlockD = new ComBlock(4,0, MessageType.CarOnlymsg);
    private static ComBlock ComBlockE = new ComBlock(3,2, MessageType.normalmsgBus);
    private static ComBlock ComBlockF = new ComBlock(4,2, MessageType.normalmsg);

    public static void AssignTomanager(LaneCommunicator communicator, LaneId laneId)
    {
        switch (laneId)
        {
            case LaneId.A:
                AddToComblock(ref ComBlockA, ref communicator);
                break;

            case LaneId.B:
                AddToComblock(ref ComBlockB, ref communicator);
                break;

            case LaneId.C:
                AddToComblock(ref ComBlockC, ref communicator);
                break;

            case LaneId.D:
                AddToComblock(ref ComBlockD, ref communicator);
                break;

            case LaneId.E:
                AddToComblock(ref ComBlockE, ref communicator);
                break;

            case LaneId.F:
                AddToComblock(ref ComBlockF, ref communicator);
                break;

            default:
                return;
        }
    }

    public static void AddToComblock(ref ComBlock block, ref LaneCommunicator laneCommunicator)
    {
        block.AddToList(ref laneCommunicator);
    }
    public static string testprint()
    {
        return "i am here and static";
    }
    public static blockmsgCarOnly GetCarmsg()
    {

        blockmsgCarOnly carOnly = ComBlockA.GetblockmsgCar();

        foreach (var item in carOnly.LCarSensormsgs)
        {
            Debug.Log(item.DetectFar + "detectednear log" + item.DetectNear + "look");
        }
        return ComBlockA.GetblockmsgCar();
    }

    public static SignalGroup GetSignalGroup()
    {
        SignalGroup signalmessage = new SignalGroup();

        signalmessage.blocksMsg1 = GetBlocks();
        signalmessage.blocksMsg2 = GetBlocksB();

        return signalmessage;
    }

    public static blocksMsg GetBlocks()
    {
        blocksMsg blocks = new blocksMsg();

        blocks.A = ComBlockA.GetNormalBlock();
        blocks.B = ComBlockB.GetNormalBlockBUS();
        blocks.C = ComBlockC.GetblockmsgCar();


        return blocks;
    }
    public static blocksMsg2 GetBlocksB()
    {

        blocksMsg2 blocks = new blocksMsg2();

        blocks.D = ComBlockD.GetblockmsgCar();
        blocks.E = ComBlockE.GetNormalBlockBUS();
        blocks.F = ComBlockF.GetNormalBlock();

        return blocks;
    }



}

public class ComBlock
{
    //the max car com to acummilate
    int maxCarCum;
    
    int maxBikeCum;

    int maxPedCum;

    bool IsCaronly;
    private MessageType messageType;

    //Car lanes can parse busses and prio vehicles
    List<LaneCommunicator> communicatorsCars = new List<LaneCommunicator>();
    //optional add only when needed
    List<LaneCommunicator> communicatorsBikes = new List<LaneCommunicator>();
    List<LaneCommunicator> communicatorsPeds = new List<LaneCommunicator>();

    List<int> Bussnumbers = new List<int>();

    public ComBlock(int maxCarCum, int maxBikeCum, MessageType messageType)
    {
        this.maxCarCum = maxCarCum;
        this.maxBikeCum = maxBikeCum;
        this.messageType = messageType;
        //CreateContainerList(ref communicatorsCars, maxCarCum);
        //CreateBikeAndPedlists();
    }

    public blockmsg GetNormalBlock()
    {

        blockmsg normal = new blockmsg();

        foreach (var item in communicatorsCars)
        {
            normal.LCarSensormsgs.Add(item.GetLaneInfoContainer().GetCarSensormsg());
        }
        foreach (var item in communicatorsBikes)
        {
            normal.Bikers.Add(item.GetLaneInfoContainer().singledetector);
        }
        foreach (var item in communicatorsPeds)
        {
            normal.Walkers.Add(item.GetLaneInfoContainer().singledetector);
        }



        return normal;
    }
    public blockmsgBus GetNormalBlockBUS()
    {

        blockmsgBus normal = new blockmsgBus();

        foreach (var item in communicatorsCars)
        {
            normal.LCarSensormsgs.Add(item.GetLaneInfoContainer().GetCarSensormsg());
        }
        foreach (var item in communicatorsBikes)
        {
            normal.Bikers.Add(item.GetLaneInfoContainer().singledetector);
        }
        foreach (var item in communicatorsPeds)
        {
            normal.Walkers.Add(item.GetLaneInfoContainer().singledetector);
        }

        normal.LBusses = Bussnumbers;


        return normal;
    }

    public blockmsgCarOnly GetblockmsgCar()
    {
        blockmsgCarOnly blockmsgCarOnly = new blockmsgCarOnly();
        foreach (var item in communicatorsCars)
        {
            blockmsgCarOnly.LCarSensormsgs.Add(item.GetLaneInfoContainer().GetCarSensormsg());
        }

        return blockmsgCarOnly;
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

    void CreateContainerList(ref List<LaneCommunicator> communicators, int maxAmount)
    {
        communicators = new List<LaneCommunicator>();
        for (int i = 0; i < maxAmount; i++)
        {
            communicators.Add(null);
        }
    }

    public void AddToList(ref LaneCommunicator laneCommunicator)
    {
        switch (laneCommunicator.type)
        {
            case NodeType.Pedestrian:
                Debug.Log("adding new pedestrian");
                communicatorsPeds.Add(laneCommunicator);
                //communicatorsPeds[laneCommunicator.GetComlaneNr()] = laneCommunicator;
                break;
            case NodeType.Biker:
                Debug.Log("adding new bike");
                communicatorsBikes.Add(laneCommunicator);
                //communicatorsBikes[laneCommunicator.GetComlaneNr()] = laneCommunicator;
                break;
            case NodeType.HighSpeed:
                Debug.Log("adding new car");
                communicatorsCars.Add(laneCommunicator);
                //communicatorsCars[laneCommunicator.GetComlaneNr()] = laneCommunicator;
                break;
            default:
                break;
        }
    }

}