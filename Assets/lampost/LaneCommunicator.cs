
using UnityEngine;

public enum LaneId
{
    A, B, C, D, E, F
}

//de lane communicator zit op de laan en beheert trigger data

//this is only for sending detection data not for getting lamp states
public class LaneCommunicator : MonoBehaviour
{
    public NodeType type = NodeType.HighSpeed;

    [SerializeField]
    LaneId id = LaneId.A;
    [SerializeField]
    int detectorNum;

    //by default
    public RoadNode NearWaitAndDetectLus;

    //when it is a high speed path
    public RoadNode FarWaitAndDetectLus;

    public bool HasBothLusses = false;

    private LaneInfoContainer laneInfoContainer = new LaneInfoContainer();

    private bool TriggerArePlaced = false;

    private void Start()
    {
        SetupDetection();
        SensorDatamanager.AssignTomanager(this, id);
    }

    public int GetComlaneNr()
    {
        return detectorNum;
    }

    public LaneId GetLaneId()
    {
        return id;
    }

    private void SetupDetection()
    {

        if(NearWaitAndDetectLus != null)
        {
            NearWaitAndDetectLus.OnTriggerChange += NearWaitAndDetectLus_OnTriggerChange;
        }
        if(FarWaitAndDetectLus != null)
        {
            FarWaitAndDetectLus.OnTriggerChange += FarWaitAndDetectLus_OnTriggerChange;
            HasBothLusses = true;
        }
        else
        {
            HasBothLusses = false;
        }

        laneInfoContainer = CreateContainer();
        laneInfoContainer.SetupInfoContainer(HasBothLusses);

    }

    private void FarWaitAndDetectLus_OnTriggerChange(bool obj)
    {
        if(laneInfoContainer.carSensormsg != null)
        {
            laneInfoContainer.carSensormsg.DetectFar = obj;
            return;

        }
        else
        {
            laneInfoContainer.carSensormsg = new CarSensormsg();
            laneInfoContainer.carSensormsg.DetectFar = obj;
        }
        //laneInfoContainer.carSensormsg.DetectFar = obj;
    }

    private void NearWaitAndDetectLus_OnTriggerChange(bool obj)
    {
        laneInfoContainer.UpdateNearLus(obj);
    }

    private LaneInfoContainer CreateContainer()
    {
        laneInfoContainer = new LaneInfoContainer();

        return laneInfoContainer;
    }



    private void OnDisable()
    {
        if (NearWaitAndDetectLus != null)
        {
            NearWaitAndDetectLus.OnTriggerChange -= NearWaitAndDetectLus_OnTriggerChange;
        }
        if (FarWaitAndDetectLus != null)
        {
            FarWaitAndDetectLus.OnTriggerChange -= FarWaitAndDetectLus_OnTriggerChange;
        }
    }

    public LaneInfoContainer GetLaneInfoContainer()
    { return laneInfoContainer; }
}

public class LaneInfoContainer
{
    // Indicates whether this lane uses a single detector (true) or multiple detectors (false)
    public bool IsSingleDetector;

    // Flags for near and far detection (optional, based on context)
    public bool Nearlus;

    public bool Farlus;

    // Flag indicating priority status (optional, based on context)
    public bool isPrio;

    // Message containing car sensor data (used when not a single detector)
    public CarSensormsg carSensormsg;

    // Single detector instance (used when IsSingleDetector is true)
    public SingleDetector singledetector;

    /// <summary>
    /// Sets up the lane information container based on whether it's a single detector.
    /// </summary>
    /// <param name="isSingleDetector">True if it's a single detector lane, otherwise false.</param>
    /// <returns>The updated LaneInfoContainer.</returns>
    public LaneInfoContainer SetupInfoContainer(bool isSingleDetector)
    {
        IsSingleDetector = isSingleDetector;
        if (!isSingleDetector)
        {
            carSensormsg = new CarSensormsg();
        }
        else
        {
            singledetector = new SingleDetector();
        }
        return this;
    }

    /// <summary>
    /// Updates the near detection status.
    /// </summary>
    public void UpdateInfo(bool _NearLus) => Nearlus = _NearLus;

    /// <summary>
    /// Updates both near and far detection statuses.
    /// </summary>
    public void UpdateInfo(bool _NearLus, bool _Farlus)
    {
        Farlus = _Farlus;
    }

    /// <summary>
    /// Gets the single detector instance.
    /// </summary>
    public SingleDetector GetSingleDetector => singledetector;

    /// <summary>
    /// Creates and returns a car sensor message based on current statuses.
    /// </summary>
    public CarSensormsg GetCarSensormsg()
    {
        if(carSensormsg == null)
        {
            return new CarSensormsg();
        }
        return carSensormsg;
    }

    public void UpdateNearLus(bool status)
    {
        if (IsSingleDetector)
        {
            singledetector.Detected = status;
        }
        else
        {
            carSensormsg.DetectNear = status;
        }
    }

    /// <summary>
    /// Clears any stored data (placeholder method).
    /// </summary>
    public void ClearData()
    {
        // Placeholder: Implement data clearing logic if needed.
    }
}

