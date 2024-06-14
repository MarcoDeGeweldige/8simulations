using System.Collections.Generic;
using UnityEngine;
public enum NodeType
{
    Pedestrian,
    Biker,
    HighSpeed
}
public class NodeTrafficSpawn : MonoBehaviour
{


    public static List<RoadNode> EntrryNodesPedest = new List<RoadNode>();
    public static List<RoadNode> EntrryNodesBiker = new List<RoadNode>();
    //busses share the entry nodes for high speed
    public static List<RoadNode> EntrryNodesHighSpeed = new List<RoadNode>();


    


    public static void AssignToEntryNode(RoadNode node, NodeType type)
    {
        switch (type)
        {
            case NodeType.Pedestrian:
                EntrryNodesPedest.Add(node);
                break;
            case NodeType.Biker:
                EntrryNodesBiker.Add(node);
                break;
            case NodeType.HighSpeed:
                EntrryNodesHighSpeed.Add(node);
                break;
            default:
                break;
        }
    }

    public static void SpawnTrafficAtAllStartPoint()
    {

        foreach (var item in EntrryNodesHighSpeed)
        {
            Instantiate(CarActorCollection.getCarPrefab(), item.GetPointPosition(), Quaternion.identity);
        }
        foreach (var item in EntrryNodesPedest)
        {

        }
        foreach(var item in EntrryNodesBiker)
        {

        }
    }

}
