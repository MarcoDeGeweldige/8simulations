using System.Collections.Generic;
using UnityEngine;

public class NodeToRoadEntry : MonoBehaviour
{
    [SerializeField]
    private NodeRoad NodeRoad; // Reference to a NodeRoad object (not shown in this snippet).

    [SerializeField]
    private bool hasroute = false; // Flag to track whether a route exists.

    private void Start()
    {
        if (NodeRoad == null)
        {
            hasroute = false; // No NodeRoad assigned, so no route.
        }
        else
        {
            hasroute = true; // NodeRoad assigned, indicating a route is available.
        }
    }

    public bool HasNewRoute()
    {
        return hasroute;
    }

    public List<Vector3> getNewRoute(ActorPathFinding actorPathFinding)
    {
        return NodeRoad.GetRoute(); // Returns the route from the NodeRoad.
    }

    public void OnTriggerEnter(Collider other)
    {
        if (hasroute)
        {
            ActorInfo actorInfo = other.gameObject.GetComponent<ActorInfo>();
            ActorPathFinding actorPath = other.gameObject.GetComponent<ActorPathFinding>();
            actorPath.SetResetAndSetRoute(this.NodeRoad.GetRoute());
        }
        //ActorInfo actorInfo = other.gameObject.GetComponent<ActorInfo>();
        //ActorPathFinding actorPath = other.gameObject.GetComponent<ActorPathFinding>();
        //actorPath.SetResetAndSetRoute(this.NodeRoad.GetRoute());
    }
}