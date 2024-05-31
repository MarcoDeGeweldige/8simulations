using System;
using UnityEngine;

//de lane bridge zit op de actor
public class LaneBridge : MonoBehaviour
{
    // Private fields
    private bool isWaitPoint = false; // Indicates whether this bridge is a wait point

    private UniversalTrigger universalTrigger = null; // Reference to the connected UniversalTrigger

    // Public properties
    public bool IsWaitPoint
    {
        get { return isWaitPoint; }
        set
        {
            // When the IsWaitPoint property is set, invoke the PassUpdate event
            IsWaitNode.Invoke(value);

            isWaitPoint = value;
        }
    }

    // Events

    public event Action<bool> IsWaitNode; // Event triggered when IsWaitPoint is queried

    public event Action<bool, bool> HasGreenLight; // Event triggered when green light status changes

    public event Action<LaneBridge> OnConnected; // Event triggered when connected to UniversalTrigger

    public SingleDetector GetSingleDetector()
    {
        return this.universalTrigger.GetSingleDetector();
    }

    public bool GetTriggerStatus()
    {
        return this.universalTrigger.GetTriggerStatus();
    }

    // Connects this LaneBridge to a UniversalTrigger
    public void ConnectVehicle(UniversalTrigger trigger)
    {
        universalTrigger = trigger;
        IsNeedsWaitNode(trigger.IsWaitPoint); // Check if this bridge needs to wait
        universalTrigger.GetLampWatch().OnCanGoChanged += LaneBridge_OnCanGoChanged; // Subscribe to CanGoChanged event
    }

    public void DisconnectTrigger()
    {
        this.isWaitPoint = false;
        if (universalTrigger != null)
        {
            universalTrigger.GetLampWatch().OnCanGoChanged -= LaneBridge_OnCanGoChanged;
        }
    }

    // Invoked when CanGo status changes in the connected UniversalTrigger
    private void LaneBridge_OnCanGoChanged(bool canGo, bool hasBus)
    {
        // Trigger the HasGreenLight event
        HasGreenLight?.Invoke(canGo, hasBus);
    }

    // Invoked to check if this bridge needs to wait
    private void IsNeedsWaitNode(bool needsWait)
    {
        IsWaitNode?.Invoke(needsWait);
    }

    // Unsubscribe from events when this component is disabled
    private void OnDisable()
    {
        if (universalTrigger != null)
        {
            universalTrigger.GetLampWatch().OnCanGoChanged -= LaneBridge_OnCanGoChanged;
        }
    }
}