using System;
using UnityEngine;


//the next step for ai
//true node based navigation
public class RoadNode : MonoBehaviour
{

    public bool IsWorldEntryNode = false;
    public NodeType type;

    //traffic light handling
    public bool IsSTopPoint = false;


    public bool IsFarLus = false;
    //end of the world will despawn
    public bool ISWorldBound = false;
    //allows vehicles to switch to other lanes
    public bool CanSwitchLaneAtNode = false;
    //this means that the next node can take busses
    public bool IsBusLaneEntryNode = false;
    //when exclusive for busses
    public bool IsBusOnlyNode = false;

    //the position of the next node
    public Vector3 NextPoint = Vector3.zero;

    //assign this to enable routing 
    public RoadNode NextNode;
    //when allowing car to switch
    public RoadNode SwitchLaneNode;

    //when a bus deviates
    public RoadNode BusNode;

    public LampWatch LampWatch;

    public bool IsDetector;

    public event Action<bool> OnTriggerChange;


    private void Awake()
    {
        this.GetComponent<BoxCollider>().isTrigger = true;
        this.GetComponent<MeshRenderer>().enabled = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        if (System.Object.ReferenceEquals(null, LampWatch))
        {
            this.IsSTopPoint = false;

        }
        else
        {
            this.IsSTopPoint = true;
        }
        if (NextNode != null)
        {
            NextPoint = NextNode.GetPointPosition();

        }
        else
        {
            NextPoint = GetPointPosition() + (transform.forward * 50);
        }
        if (IsBusLaneEntryNode)
        {
            BusNode = FindBusNode();
        }
        if (NextNode != null && SwitchLaneNode != null)
        {
            this.CanSwitchLaneAtNode = true;

        }

        if (!IsWorldEntryNode)
        {
            if (NextNode != null)
            {
                NextPoint = NextNode.GetPointPosition();

            }
            else
            {
                NextPoint = GetPointPosition() + (transform.forward * 50);
            }
            if (IsBusLaneEntryNode)
            {
                BusNode = FindBusNode();
            }
            if (NextNode != null && SwitchLaneNode != null)
            {
                this.CanSwitchLaneAtNode = true;

            }
        }
        else
        {
            //world spawn here, set lane as an public thing to spawn
            NodeTrafficSpawn.AssignToEntryNode(this, type);

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Actor"))
        {
            other.GetComponent<ActorNodeReader>().ReadNodeInfo(this);
            if (this.ISWorldBound)
            {
                other.GetComponentInChildren<ActorNodeReader>().DisableActor();
            }

            if (IsDetector)
            {
                OnTriggerChange.Invoke(true);
            }
        }
        else
        {
            return;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (IsDetector)
        {
            OnTriggerChange.Invoke(true);
        }
    }

    public Vector3 GetPointPosition()
    {
        return this.transform.position;
    }
    //get the positiof the next node
    public Vector3 GetNextPosition()
    {

        if (CanSwitchLaneAtNode)
        {
            int c = UnityEngine.Random.Range(1, 3);

            if (c == 1)
            {
                return this.NextPoint;
            }
            else
            {

                return this.SwitchLaneNode.GetPointPosition();
            }

        }
        return this.NextPoint;
    }

    public void SetUpTriggers()
    {

    }
    public Vector3 GetNextBusPosition()
    {
        return this.BusNode.GetPointPosition();
    }


    public ref LampWatch GetLampWatch()
    {
        return ref LampWatch;
    }

    private RoadNode FindBusNode()
    {
        if (NextNode.IsBusOnlyNode)
        {
            return NextNode;
        }
        if (SwitchLaneNode.IsBusOnlyNode)
        {
            return SwitchLaneNode;
        }
        return NextNode;
    }

    
}
