using System.Collections.Generic;
using System.Linq;

public enum MessageType
{
    normalmsg,
    normalmsgBus,
    CarOnlymsg
}

public static class SensorDatamanager
{
    private static ComBlock ComBlockA = new ComBlock(4, 2, MessageType.normalmsg);
    private static ComBlock ComBlockB = new ComBlock(4, 2, MessageType.normalmsgBus);
    private static ComBlock ComBlockC = new ComBlock(4, 0, MessageType.CarOnlymsg);
    private static ComBlock ComBlockD = new ComBlock(4, 0, MessageType.CarOnlymsg);
    private static ComBlock ComBlockE = new ComBlock(3, 2, MessageType.normalmsgBus);
    private static ComBlock ComBlockF = new ComBlock(4, 2, MessageType.normalmsg);

    private static List<LampostManager> standalonelampB = new List<LampostManager>();
    private static List<LampostManager> standalonelampE = new List<LampostManager>();

    public static void AssignTomanager(LaneCommunicator communicator, LaneId laneId)
    {
        switch(laneId)
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

    public static void AddNumberToBuslist(int num, LaneId laneId)
    {
        switch(laneId)
        {
            case LaneId.B:
                RegisterLineToLane(ref ComBlockB, num);
                break;

            case LaneId.E:
                RegisterLineToLane(ref ComBlockE, num);
                break;

            default:
                return;
        }
    }

    public static void AddLightToList(LampostManager lampost, bool isB)
    {
        if(isB)
        {
            standalonelampB.Add(lampost);
        }
        else
        {
            standalonelampE.Add(lampost);
        }
    }

    public static void assignToIsolatedLight(ref recieverpakket.SignalGroup fullpakket)
    {
        standalonelampB[0].SetLight(fullpakket.blocksMsg.B.busje[0]);

        standalonelampE[0].SetLight(fullpakket.blocksMsg2.E.busje[0]);
        standalonelampE[1].SetLight(fullpakket.blocksMsg2.E.busje[1]);
    }

    public static void RemoveNumberToBuslist(int num, LaneId laneId)
    {
        switch(laneId)
        {
            case LaneId.B:
                RemoveLineToLane(ref ComBlockB, num);
                break;

            case LaneId.E:
                RemoveLineToLane(ref ComBlockE, num);
                break;

            default:
                return;
        }
    }

    public static void RegisterLineToLane(ref ComBlock block, int number)
    {
        block.AddBusNumberToList(number);
    }

    public static void RemoveLineToLane(ref ComBlock block, int number)
    {
        block.RemoveBusnumberFromList(number);
    }

    public static void AddToComblock(ref ComBlock block, ref LaneCommunicator laneCommunicator)
    {
        block.AddToList(ref laneCommunicator);
    }

    public static blockmsgCarOnly GetCarmsg()
    {
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
    private int maxCarCum;

    private int maxBikeCum;

    private int maxPedCum;

    private bool IsCaronly;
    private MessageType messageType;

    //Car lanes can parse busses and prio vehicles
    private List<LaneCommunicator> communicatorsCars = new List<LaneCommunicator>();

    //optional add only when needed
    private List<LaneCommunicator> communicatorsBikes = new List<LaneCommunicator>();

    private List<LaneCommunicator> communicatorsPeds = new List<LaneCommunicator>();

    private List<int> Bussnumbers = new List<int>();

    public ComBlock(int maxCarCum, int maxBikeCum, MessageType messageType)
    {
        this.maxCarCum = maxCarCum;
        this.maxBikeCum = maxBikeCum;
        this.messageType = messageType;
        CreateContainerList(ref communicatorsCars, maxCarCum);
        CreateBikeAndPedlists();
    }

    public void AddBusNumberToList(int num)
    {
        if(Bussnumbers.Contains(num))
        {
            return;
        }
        Bussnumbers.Add(num);
    }

    public void RemoveBusnumberFromList(int num)
    {
        if(Bussnumbers.Contains(num))
        {
            Bussnumbers.Remove(num);
        }
    }

    public blockmsg GetNormalBlock()
    {
        blockmsg normal = new blockmsg();
        normal.LCarSensormsgs.AddRange(communicatorsCars.Select(item => item.GetLaneInfoContainer().GetCarSensormsg()));
        normal.Bikers.AddRange(communicatorsBikes.Select(item => item.GetLaneInfoContainer().singledetector));
        normal.Walkers.AddRange(communicatorsPeds.Select(item => item.GetLaneInfoContainer().singledetector));
        return normal;
    }

    public blockmsgBus GetNormalBlockBUS()
    {
        blockmsgBus normal = new blockmsgBus();
        normal.LCarSensormsgs.AddRange(communicatorsCars.Select(item => item.GetLaneInfoContainer().GetCarSensormsg()));
        normal.Bikers.AddRange(communicatorsBikes.Select(item => item.GetLaneInfoContainer().singledetector));
        normal.Walkers.AddRange(communicatorsPeds.Select(item => item.GetLaneInfoContainer().singledetector));
        normal.LBusses = Bussnumbers;

        return normal;
    }

    public blockmsgCarOnly GetblockmsgCar()
    {
        blockmsgCarOnly blockmsgCarOnly = new blockmsgCarOnly();
        blockmsgCarOnly.LCarSensormsgs.AddRange(communicatorsCars.Select(item => item.GetLaneInfoContainer().GetCarSensormsg()));
        return blockmsgCarOnly;
    }

    private void CreateBikeAndPedlists()
    {
        if(maxBikeCum > 0)
        {
            maxPedCum = maxBikeCum * 2;
            CreateContainerList(ref communicatorsBikes, maxBikeCum);
            CreateContainerList(ref communicatorsPeds, maxPedCum);
        }
    }

    private void CreateContainerList(ref List<LaneCommunicator> communicators, int maxAmount)
    {
        communicators = new List<LaneCommunicator>();
        for(int i = 0; i < maxAmount; i++)
        {
            communicators.Add(null);
        }
    }

    public void AddToList(ref LaneCommunicator laneCommunicator)
    {
        switch(laneCommunicator.type)
        {
            case NodeType.Pedestrian:
                if(laneCommunicator.GetComlaneNr() <= communicatorsPeds.Count - 1)
                {
                    communicatorsPeds[laneCommunicator.GetComlaneNr()] = laneCommunicator;
                }

                break;

            case NodeType.Biker:
                if(laneCommunicator.GetComlaneNr() <= communicatorsBikes.Count - 1)
                {
                    communicatorsBikes[laneCommunicator.GetComlaneNr()] = laneCommunicator;
                }
                break;

            case NodeType.HighSpeed:
                if(laneCommunicator.GetComlaneNr() <= communicatorsCars.Count - 1)
                {
                    communicatorsCars[laneCommunicator.GetComlaneNr()] = laneCommunicator;
                }

                break;

            default:
                break;
        }
    }
}