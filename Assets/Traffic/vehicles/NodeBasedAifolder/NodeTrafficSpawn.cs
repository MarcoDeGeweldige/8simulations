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

}
