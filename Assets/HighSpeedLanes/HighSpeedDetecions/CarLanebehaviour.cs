using UnityEngine;

/// <summary>
/// Manages behavior related to a car lane.
/// </summary>
public class CarLanebehaviour : MonoBehaviour
{
    // Public fields
    public bool HasOffsetNearlusFromStart = false;

    public float EntranceOffset = 0;
    public Vector3 OffsettedStartPos;
    public bool IsMiddlePoint = false;
    public GameObject ExitNode;
    //public LTrigger NearLus; // Signal detector at the signal (near)
    //public LTrigger FarLus; // Signal detector far from the signal
    public CarSensormsg DetectorLus = new CarSensormsg();
    public float FarLaneDistance; // Offset far from the signal light
    public float LaneStartdistance; // Extra offset added on top of the far lane (true beginning of the road)
    private Road LaneRoad; // Local road definition
    public LampostManager LampostManager;

    LaneCommunicator communicator;

    public RoadNode nearlus;
    public RoadNode farlus;


    // Start method initializes detectors and other settings
    private void Start()
    {
        communicator = this.GetComponentInParent<LaneCommunicator>();

        if(communicator.FarWaitAndDetectLus != null)
        {
            farlus = communicator.FarWaitAndDetectLus;

        }
        if (communicator.NearWaitAndDetectLus != null)
        {
            nearlus = communicator.NearWaitAndDetectLus;
        }
        LaneRoad = GetComponentInChildren<Road>();
        if (nearlus)
        {
            Debug.Log("i got an lus");
        }
        else
        {
            Debug.Log("nolus");
        }

        if(nearlus != null)
        {
            nearlus.OnTriggerChange += updateNearlus;
            nearlus.IsDetector = true;
            nearlus.LampWatch = LampostManager.watch;
            nearlus.IsSTopPoint = true;
            Debug.Log("nearlus is detector");
        }
        if(farlus != null)
        {
            farlus.OnTriggerChange += updateFarLus;
            farlus.IsDetector = true;
            Debug.Log("nearlus is detector");
        }


        // Adjust position of the far detector
        //FarLus.gameObject.transform.position += FarLus.transform.forward * FarLaneDistance;

        // Set up near and far detectors
        //NearLus.setup(this, true);
        //FarLus.setup(this, false);

        // Calculate offsetted start position
        //if (HasOffsetNearlusFromStart)
        //{
        //    OffsettedStartPos = NearLus.transform.position + NearLus.transform.forward * EntranceOffset;
        //}
    }


    public void updateNearlus(bool state, bool isPrio)
    {

        this.DetectorLus.DetectNear = state;
    }

    public void updateFarLus(bool state, bool isPrio)
    {
        this.DetectorLus.DetectFar = state;
    }
    // Set this lane as a middle point
    public void SetAsmiddlePoint()
    {
        this.IsMiddlePoint = true;
    }

    // Handle detection events
    public void OnDetect(bool isnear)
    {
        if (isnear)
        {
            DetectorLus.DetectNear = true;
        }
        else
        {
            DetectorLus.DetectFar = true;
        }
    }

    // Handle detection events with priority vehicle flag
    public void OnDetect(bool isnear, bool IsprioVehicle)
    {
        if (isnear)
        {
            DetectorLus.DetectNear = true;
            DetectorLus.PrioCar = true;
        }
        else
        {
            DetectorLus.DetectFar = true;
            DetectorLus.PrioCar = true;
        }
    }

    // Handle exit detection events
    public void ExitDetected(bool isnear)
    {
        if (isnear)
        {
            DetectorLus.DetectNear = false;
        }
        else
        {
            DetectorLus.DetectFar = false;
        }
    }

    // Handle exit detection events with priority vehicle flag
    public void ExitDetected(bool isnear, bool IsprioVehicle)
    {
        if (isnear)
        {
            DetectorLus.DetectNear = false;
            DetectorLus.PrioCar = false;
        }
        else
        {
            DetectorLus.DetectFar = false;
        }
    }

    // Set lamp light state
    public void SetLampLight(int state)
    {
        LampostManager.SetLight(state);


    }


    // Get trigger information
    public CarSensormsg getTriggerInfo()
    {
        return this.DetectorLus;
    }

    private void OnDisable()
    {
        if(nearlus != null)
        {
            nearlus.OnTriggerChange -= updateNearlus;
        }
        if (farlus != null)
        {
            farlus.OnTriggerChange -= updateFarLus;
        }
    }
}

