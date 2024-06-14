using UnityEngine;


//handle the information of nodes 
public class ActorNodeReader : MonoBehaviour
{


    public Vector3 NextNodePos;

    public bool IsWaitPoint;

    public MovementM2 MovementM2;

    public bool IsBus;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MovementM2 = this.GetComponent<MovementM2>();
    }

    public void SetIfBus(bool isBus)
    {
        IsBus = isBus;
    }

    public void ReadNodeInfo(RoadNode node)
    {
        if (node.IsSTopPoint)
        {
            SetLampWatch(ref node.GetLampWatch());
        }
        if (IsBus)
        {
            if (node.IsBusLaneEntryNode)
            {
                MovementM2.SetNewGoal(node.GetNextBusPosition());
                return;
            }
        }
        MovementM2.SetNewGoal(node.GetNextPosition());
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
