using System.Collections.Generic;
using UnityEngine;

public static class SensorDatamanager
{

    private static ComBlock ComBlockA = new ComBlock(4,2);
    private static ComBlock ComBlockB = new ComBlock(4,2);
    private static ComBlock ComBlockC = new ComBlock(4,0);
    private static ComBlock ComBlockD = new ComBlock(4,0);
    private static ComBlock ComBlockE = new ComBlock(3,2);
    private static ComBlock ComBlockF = new ComBlock(4,2);

    public static void AssignTomanager(LaneCommunicator communicator, LaneId laneId)
    {
        Debug.Log("added lane");
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



}

public class ComBlock
{
    //the max car com to acummilate
    int maxCarCum;
    
    int maxBikeCum;

    int maxPedCum;

    //Car lanes can parse busses and prio vehicles
    List<LaneCommunicator> communicatorsCars;
    //optional add only when needed
    List<LaneCommunicator> communicatorsBikes;
    List<LaneCommunicator> communicatorsPeds;

    public ComBlock(int maxCarCum, int maxBikeCum)
    {
        this.maxCarCum = maxCarCum;
        this.maxBikeCum = maxBikeCum;
        CreateContainerList(ref communicatorsCars, maxCarCum);
        CreateBikeAndPedlists();
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
                communicatorsPeds[laneCommunicator.GetComlaneNr()] = laneCommunicator;
                break;
            case NodeType.Biker:
                communicatorsBikes[laneCommunicator.GetComlaneNr()] = laneCommunicator;
                break;
            case NodeType.HighSpeed:
                communicatorsCars[laneCommunicator.GetComlaneNr()] = laneCommunicator;
                break;
            default:
                break;
        }
    }

}