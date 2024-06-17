using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class NodeRoad : MonoBehaviour
{
    //// List of node blocks (optional, if needed for other functionality)
    //public List<GameObject> nodeBlocks = new List<GameObject>();

    //// List to store the route waypoints
    //public List<Vector3> route = new List<Vector3>();

    //// Awake is called once before the first execution of Update after the MonoBehaviour is created
    //private void Awake()
    //{
    //    InitializeRoute();
    //}

    //// Initializes the route by collecting positions from child transforms
    //private void InitializeRoute()
    //{
    //    route.Clear(); // Clear the list to avoid duplicates

    //    // Get all child transforms (including the current transform)
    //    Transform[] childTransforms = GetComponentsInChildren<Transform>();

    //    foreach (Transform child in childTransforms)
    //    {
    //        // Add the position of each child to the route
    //        route.Add(child.position);
    //    }
    //    route.Remove(route.First());
    //}

    //// Returns the route waypoints
    //public List<Vector3> GetRoute()
    //{
    //    return route;
    //}
}