using UnityEngine;


//handle the information of nodes 
public class ActorNodeReader : MonoBehaviour
{


    public Vector3 NextNodePos;

    public bool IsWaitPoint;

    public MovementM2 MovementM2;

    public bool IsBus;
    public int bunLineNumber;
    public bool IsPrioV = false;

    public NodeType ActorType;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MovementM2 = this.GetComponent<MovementM2>();
        IsPrioV = MovementM2.IsPrio;
        if (IsBus)
        {
            bunLineNumber = GetNewBusNumber();
        }
    }

    public void SetActorType(NodeType type)
    {
        ActorType = type;
    }

    public void SetIfBus(bool isBus)
    {
        IsBus = isBus;
        this.bunLineNumber = GetNewBusNumber();
    }

    public int GetNewBusNumber()
    {
        return Random.Range(0,9000);
    }

    public int GetBunNumber()
    {
        return this.bunLineNumber;
    }

    public bool ReadNodeInfo(RoadNode node)
    {

        if(node.type == ActorType)
        {
            if (node.ISWorldBound)
            {
                DisableActor();
            }
        }
        else
        {
            return false;
        }
        if (node.IsSTopPoint)
        {
            SetLampWatch(ref node.GetLampWatch());
        }
        if (IsBus)
        {
            if (node.IsBusLaneEntryNode)
            {
                MovementM2.SetNewGoal(node.GetNextBusPosition());
            }
        }
        MovementM2.SetNewGoal(node.GetNextPosition());
        //node.get
        return true;
    }

    public void SetLampWatch(ref LampWatch watch)
    {
        MovementM2.SetLampWatch(ref watch);
    }


    public void DisableActor()
    {
        this.gameObject.SetActive(false);
    }
}
