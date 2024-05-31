using System;
using Unity.VisualScripting;
using UnityEngine;

public class UniversalTrigger : MonoBehaviour
{
    public Vector3 DetectorSize;
    //needs to wait when true
    public bool IsWaitPoint;

    public bool hasActor = false;
    public SingleDetector MySingleDetector = new SingleDetector();
    //this is the initial lanebridge
    private LaneBridge laneBridge;

    private LampWatch LampWatch;


    public event Action<bool> OnTriggerChange;
    public event Action<bool, bool> OnlampChange;


    public void Setup()
    {
        LampWatch = this.GetComponentInChildren<LampWatch>();
        LampWatch = this.GetComponentInParent<LampWatch>();
        //LampWatch.OnCanGoChanged += LampWatch_OnCanGoChanged;
        BoxCollider collider = this.AddComponent<BoxCollider>();
        collider.isTrigger = true;
#pragma warning disable IDE0054 // Use compound assignment
        collider.size = collider.size * 2;
#pragma warning restore IDE0054 // Use compound assignment

    }

    public void Setup(LaneBridge bridge)
    {
        laneBridge = bridge;

    }

    public void OnTriggerEnter(Collider other)
    {
        ActorInfo actorInfo = other.gameObject.GetComponent<ActorInfo>();
        this.laneBridge = other.gameObject.GetComponent<LaneBridge>();
        laneBridge.IsWaitPoint = this.IsWaitPoint;
        laneBridge.ConnectVehicle(this);
        MySingleDetector.Detected = true;

        Debug.Log(actorInfo.Type + "is waiting");

        if (actorInfo != null)
        {
            OnTriggerChange?.Invoke(true);

        }
    }
    public bool GetTriggerStatus()
    {
        return hasActor;
    }
    public SingleDetector GetSingleDetector()
    {
        return MySingleDetector;
    }

    public void OnTriggerExit(Collider other)
    {
        ActorInfo actorInfo = other.gameObject.GetComponent<ActorInfo>();
        OnTriggerChange?.Invoke(false);
        laneBridge.DisconnectTrigger();
        MySingleDetector.Detected = false;


    }

    public LampWatch GetLampWatch()
    {
        return this.LampWatch;
    }

}