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
            int spawn = Random.Range(0, 2);

            if (spawn == 1)
            {
                Instantiate(CarActorCollection.GetRandomCarPrefab(), item.GetPointPosition(), Quaternion.identity);
            }
            //Instantiate(CarActorCollection.GetRandomCarPrefab(), item.GetPointPosition(), Quaternion.identity);
        }
        foreach (var item in EntrryNodesPedest)
        {
            int spawn = Random.Range(0, 2);

            if (spawn == 1)
            {
                Instantiate(CarActorCollection.GetRandomPedPrefab(), item.GetPointPosition(), Quaternion.identity);
            }
        }
        foreach (var item in EntrryNodesBiker)
        {
            int spawn = Random.Range(0, 2);

            if (spawn == 1)
            {
                Instantiate(CarActorCollection.GetRandomBikePrefab(), item.GetPointPosition(), Quaternion.identity);
            }
        }
    }
}