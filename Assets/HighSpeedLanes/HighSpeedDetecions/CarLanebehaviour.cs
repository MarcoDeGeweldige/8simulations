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

    public CarSensormsg DetectorLus = new CarSensormsg();

    public float FarLaneDistance; // Offset far from the signal light
    public float LaneStartdistance; // Extra offset added on top of the far lane (true beginning of the road)
    public LampostManager LampostManager;

    private LaneCommunicator communicator;

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
        if(communicator.NearWaitAndDetectLus != null)
        {
            nearlus = communicator.NearWaitAndDetectLus;
        }

        if(nearlus != null)
        {
            nearlus.OnTriggerChange += updateNearlus;
            nearlus.IsDetector = true;
            nearlus.LampWatch = LampostManager.watch;
            nearlus.IsSTopPoint = true;
        }
        if(farlus != null)
        {
            farlus.OnTriggerChange += updateFarLus;
            farlus.IsDetector = true;
        }
    }

    public void updateNearlus(bool state, bool isPrio)
    {
        this.DetectorLus.DetectNear = state;
    }

    public void updateFarLus(bool state, bool isPrio)
    {
        this.DetectorLus.DetectFar = state;
    }

    // Set lamp light state
    public void SetLampLight(int state)
    {
        LampostManager.SetLight(state);
    }

    private void OnDisable()
    {
        if(nearlus != null)
        {
            nearlus.OnTriggerChange -= updateNearlus;
        }
        if(farlus != null)
        {
            farlus.OnTriggerChange -= updateFarLus;
        }
    }
}