using UnityEngine;

public class ActorInfo : MonoBehaviour
{
    //if 1 dan prio
    public int Type = 0;

    public int _BusNum;

    //is this a prio vehicle
    public bool Isprio()
    {
        if (Type == 1)
        {
            return true;
        }
        return false;
    }

    public bool IsCar()
    {
        if (Type == 2)
        {
            return true;
        }
        return false;
    }

    public bool IsWalker()
    {
        if (Type == 3)
        {
            return true;
        }
        return false;
    }

    public bool IsBiker()
    {
        if (Type == 4)
        {
            return true;
        }
        return false;
    }

    public bool IsBus()
    {
        if (Type == 5)
        {
            return true;
        }
        return false;
    }
}