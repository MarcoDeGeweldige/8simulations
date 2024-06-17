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

    //for interaction with needed groeps
    //public NodeType nodeType = NodeType.HighSpeed;

    //assign this to enable routing
    public RoadNode NextNode;

    //when allowing car to switch
    public RoadNode SwitchLaneNode;

    //when a bus deviates
    public RoadNode BusNode;

    public LampWatch LampWatch;

    public bool IsDetector;

    public event Action<bool, bool> OnTriggerChange;

    public event Action<int> OnBusLeave;

    public event Action<int> OnBusEnter;

    public event Action<bool, int> OnTriggerChangeWithBus;

    private void Awake()
    {
        this.GetComponent<BoxCollider>().isTrigger = true;
        this.GetComponent<MeshRenderer>().enabled = false;
        if (SwitchLaneNode != null)
        {
            this.CanSwitchLaneAtNode = true;
        }
    }

    public void AssingLampostManager(ref LampWatch watch)
    {
        if (System.Object.ReferenceEquals(null, LampWatch))
        {
            this.LampWatch = watch;
            this.IsSTopPoint = true;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        if (System.Object.ReferenceEquals(null, LampWatch))
        {
            this.IsSTopPoint = false;
        }
        else
        {
            this.IsSTopPoint = true;
            this.IsDetector = true;
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

        if (!IsWorldEntryNode)
        {
            if (NextNode != null)
            {
                NextPoint = NextNode.GetPointPosition();
            }
            else
            {
                NextPoint = GetPointPosition() + (transform.forward * 5000);
            }
            if (IsBusLaneEntryNode)
            {
                BusNode = FindBusNode();
            }
        }
        else
        {
            //world spawn here, set lane as an public thing to spawn
            NodeTrafficSpawn.AssignToEntryNode(this, type);
        }
    }

    //public Vector3 PickRandomNodeAtSwitch()
    //{
    //    if(SwitchLaneNode != null)
    //    {
    //        int spawn = UnityEngine.Random.Range(0, 2);

    //        if (spawn == 1)
    //        {
    //            return SwitchLaneNode.GetPointPosition();
    //        }

    //    }
    //    return
    //}
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Actor"))
        {
            //other.GetComponent<ActorNodeReader>().ReadNodeInfo(this);
            if (other.GetComponent<ActorNodeReader>().ReadNodeInfo(this))
            {
                ActorNodeReader actorNode = other.GetComponent<ActorNodeReader>();

                if (IsDetector)
                {
                    if (!actorNode.IsBus)
                    {
                        OnTriggerChange.Invoke(true, actorNode.IsPrioV);
                    }
                    else
                    {
                        OnTriggerChange.Invoke(true, actorNode.IsPrioV);

                        OnBusEnter?.Invoke(actorNode.GetBunNumber());

                        //OnBusEnter.Invoke(actorNode.GetBunNumber());
                    }
                }

                //if (IsDetector)
                //{
                //    OnTriggerChange.Invoke(true);
                //}
            }
            //if (this.ISWorldBound)
            //{
            //    other.GetComponentInChildren<ActorNodeReader>().DisableActor();
            //}

            //if (IsDetector)
            //{
            //    OnTriggerChange.Invoke(true);
            //}
        }
        else
        {
            return;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Actor"))
        {
            //other.GetComponent<ActorNodeReader>().ReadNodeInfo(this);
            if (other.GetComponent<ActorNodeReader>().ReadNodeInfo(this))
            {
                ActorNodeReader actorNode = other.GetComponent<ActorNodeReader>();
                if (actorNode.IsBus)
                {
                    OnBusLeave?.Invoke(actorNode.GetBunNumber());
                }
                else
                {
                    OnTriggerChange?.Invoke(false, actorNode.IsPrioV);
                }
            }
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
            int c = UnityEngine.Random.Range(0, 2);

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