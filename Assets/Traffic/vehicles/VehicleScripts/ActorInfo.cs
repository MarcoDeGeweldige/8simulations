

using UnityEngine;

public class ActorInfo : MonoBehaviour
{
    // Type represents the type of actor:
    // 0: Default (not specified)
    // 1: Priority vehicle
    // 2: Car
    // 3: Walker
    // 4: Biker
    // 5: Bus
    public int Type = 0;

    public int _BusNum; // Bus number (not used in the provided methods)

    // Check if this actor is a priority vehicle
    public bool IsPrio()
    {
        return Type == 1;
    }

    // Check if this actor is a car
    public bool IsCar()
    {
        return Type == 2;
    }

    // Check if this actor is a walker
    public bool IsWalker()
    {
        return Type == 3;
    }

    // Check if this actor is a biker
    public bool IsBiker()
    {
        return Type == 4;
    }

    // Check if this actor is a bus
    public bool IsBus()
    {
        return Type == 5;
    }
}
