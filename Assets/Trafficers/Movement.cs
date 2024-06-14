using System;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class Movement : MonoBehaviour
{
    [SerializeField] private bool isBus; // Flag to determine if this node represents a bus.

    private NodeRoad nodeRoad; // Reference to a NodeRoad object (not shown in this snippet).
    private vehicleDetector vehicleDetector; // Handles vehicle detection.
    private LaneBridge laneBridge; // Handles lane-related functionality.
    public ActorPathFinding pad; // Represents a pad (not shown in this snippet).

    private bool hasOffsetStart; // Flag indicating if the path has an offset.
    private bool hasNodeRoute; // Flag indicating if a route exists for this node.
    private int routeIndex = -1; // Index of the current route segment.

    private float currentDuration; // Current duration for long scans.

    //public int routeindex = 0;
    public bool CanUpdate = false;

    public bool Hasoffsetstart = false;
    public bool Detectedvehicle = false;
    public bool IsAtWaitNode = false;
    private bool isWaiting = false;
    public bool IsBus = false;
    private bool IsGreenLight;
    private bool _hasNodeRoute = false;

    //public event Action FinishedWalk;

    public float Maxspeed = 10;
    public float timeSinceStarted;
    public float percentageComplete;
    public float ScanPosition = 5;
    public float duration = 3.0f; // Duration of the movement in seconds
    public float CurrentDuration = 0;
    public float MaxDelayDuration = 2.0f;
    public float NodeDuration = 0;
    public float DuractionCap;
    public float durationoff = 1.0f; // Duration of the movement in seconds
    private float startTime; // Time when the movement started
    private float lastPercentageComplete;
    public Vector3 LocalGoal;
    public Vector3 startpos;

    //rigidbody pyhcis
    public Rigidbody body;

    private void Start()
    {
        if (IsBus)
        {
            this.GetComponentInParent<Transform>().transform.position += this.GetComponentInParent<Transform>().transform.up * 5;

            vehicleDetector = this.AddComponent<vehicleDetector>();
            this.vehicleDetector.OnVehcleDetected += VehicleDetector_OnVehcleDetected;
            this.vehicleDetector.OnLongScanDetected += VehicleDetector_OnLongScanDetected;

            this.CanUpdate = false; ;
            routeIndex = -1;
            SmartUpdate();
        }


    }

    private void VehicleDetector_OnLongScanDetected(bool obj)
    {
        if (obj)
        {
            CurrentDuration += 0.4f;
            CurrentDuration = Mathf.Clamp(CurrentDuration, duration, DuractionCap);
        }
    }

    public void Setup()
    {
        laneBridge = this.AddComponent<LaneBridge>();
        this.GetComponentInParent<Transform>().transform.position += this.GetComponentInParent<Transform>().transform.up * 5;
        vehicleDetector = this.AddComponent<vehicleDetector>();
        pad.watch.OnCanGoChanged += Watch_OnCanGoChanged;
        this.vehicleDetector.OnVehcleDetected += VehicleDetector_OnVehcleDetected;
        //laneBridge.IsWaitNode += Checknode;
        this.pad.OnNewRoute += Pad_OnNewRoute;
        this.CanUpdate = false;
        Hasoffsetstart = pad.HasOffsetedPath();
        routeIndex = -1;

        SmartUpdate();
    }

    private void Pad_OnNewRoute()
    {
        this.routeIndex = -1;
        if (!_hasNodeRoute)
        {
            _hasNodeRoute = true;
            this.pad.OnNewRoute += Pad_OnNewRoute;
        }
        this.pad.OnNewRoute -= Pad_OnNewRoute;
        this.pad.OnNewRoute += Pad_OnNewRoute;

        SmartUpdate();
    }

    private void LaneBridge_HasGreenLight(bool arg1, bool arg2)
    {
        throw new NotImplementedException();
    }

    private void Checknode(bool obj)
    {
        IsAtWaitNode = obj;
    }

    private void VehicleDetector_OnVehcleDetected(bool detected)
    {
        if (detected)
        {
            Detectedvehicle = detected;
        }
        else
        {
            Detectedvehicle = detected;
            CurrentDuration -= 0.1f;
            CurrentDuration = Mathf.Clamp(CurrentDuration, duration, DuractionCap);
        }
    }

    public void SetNewPadLink(LampWatch watch)
    {
        if (pad.watch != null)
        {
            pad.watch.OnCanGoChanged -= Watch_OnCanGoChanged;
        }
        pad.watch = watch;
        pad.watch.OnCanGoChanged += Watch_OnCanGoChanged;
        pad.watch.BusEnteredlane();
    }

    public void SetNewDuration(float duration)
    {
        this.duration = UnityEngine.Random.Range(duration - 1, duration + 1); ;
        CurrentDuration = this.duration;
        DuractionCap = this.duration + this.MaxDelayDuration;
    }

    public void SetObjective(Vector3 goal)
    {
        this.LocalGoal = goal;
    }

    public Vector3 GetNewLocalGoal()
    {
        return pad.GetNextPos(routeIndex);
    }

    private void BusLeftLane()
    {
        pad.watch.BussPassedLane();
    }

    private void SmartUpdate()
    {
        if (IsAtWaitNode)
        {
            if (!IsGreenLight)
            {
                this.isWaiting = true;
                return;
            }
            else
            {
                this.isWaiting = false;
            }
        }
        RestTimer();
        routeIndex++;
        //check if this is the end of the path
        //if not finished get next point
        if (pad.IsFinished(routeIndex))
        {
            this.LocalGoal = GetNewLocalGoal();
            Maxspeed = Vector3.Distance(transform.position, LocalGoal) / duration;
            //Debug.Log(Maxspeed);
            this.CanUpdate = true;
        }
        else
        {
            //done
            //stop now
            this.CanUpdate = false;
            //FinishedWalk.Invoke();
            this.gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        if (CanUpdate)
        {
            timebasedmovementX();
        }
    }

    private void Watch_OnCanGoChanged(bool cango, bool hasBus)
    {
        if (isWaiting)
        {
            if (hasBus)
            {
                if (IsBus)
                {
                    this.IsGreenLight = cango;
                    if (cango == true)
                    {
                        BusLeftLane();
                        this.isWaiting = false;
                        this.CanUpdate = cango;
                    }
                    return;
                }

                this.IsGreenLight = false;
            }
            else
            {
                this.IsGreenLight = cango;
                this.isWaiting = false;
                this.CanUpdate = cango;
            }
        }
    }

    private void OnDisable()
    {
        // Unsubscribe from the event

        if (pad.watch != null)
        {
            pad.watch.OnCanGoChanged -= Watch_OnCanGoChanged;
        }

        if (vehicleDetector != null)
        {
            this.vehicleDetector.OnVehcleDetected -= VehicleDetector_OnVehcleDetected;
            this.vehicleDetector.OnLongScanDetected -= VehicleDetector_OnLongScanDetected;
        }
        if (laneBridge != null)
        {
            //laneBridge.IsWaitNode -= Checknode;
        }
        if (_hasNodeRoute)
        {
            this.pad.OnNewRoute -= Pad_OnNewRoute;
        }
    }

    public void DisableActor()
    {
        //done
        //stop now
        this.CanUpdate = false;
        //FinishedWalk.Invoke();
        this.gameObject.SetActive(false);
    }

    public void RestTimer()
    {
        startTime = Time.time;
        startpos = transform.position;
    }

    public void timebasedmovementX()
    {
        if (isWaiting || Detectedvehicle)
        {
            return;
        }

        float timeDifference = Time.time - startTime;
        percentageComplete = timeDifference / CurrentDuration;

        // Only call Lerp if there's a need to update the position
        if (percentageComplete != lastPercentageComplete)
        {
            Vector3 TargetPos = Vector3.Lerp(startpos, LocalGoal, percentageComplete);
            transform.LookAt(LocalGoal);
            //this.body.MovePosition(TargetPos);

            this.body.MovePosition( transform.position += transform.forward * Maxspeed *Time.deltaTime);
            lastPercentageComplete = percentageComplete;
        }
        if (percentageComplete >= 1.0f)
        {
            SmartUpdate();
        }
    }
}