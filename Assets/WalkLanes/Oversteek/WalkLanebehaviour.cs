using UnityEngine;

public class WalkLanebehaviour : MonoBehaviour
{
    public SingleDetector detectorLus;
    public float laneStartDistance;

    //public GameObject DetectionNear;
    //public SingleDetector DetectorLus = new SingleDetector();

    public LampostManager lampostManager;
    public bool IsMiddlePoint = false;
    private Road laneRoad;

    LaneCommunicator communicator;

    public RoadNode nearlus;

    // Start is called before the first frame update
    private void Start()
    {
        laneRoad = GetComponentInChildren<Road>();


        communicator = this.GetComponent<LaneCommunicator>();

        if (communicator.NearWaitAndDetectLus != null)
        {
            nearlus = communicator.NearWaitAndDetectLus;

        }

        if (nearlus)
        {
            Debug.Log("i got an lus");
        }
        else
        {
            Debug.Log("nolus");
        }

        if (nearlus != null)
        {
            nearlus.IsDetector = true;
            nearlus.IsSTopPoint = true;
            nearlus.LampWatch = this.lampostManager.watch;
            Debug.Log("nearlus is detector");
        }


    }


    
    public Vector3 GetLaneStartSignal()
    {
        return laneRoad.GetStartPosition().position;
    }

    public Vector3 GetLaneExit()
    {
        return laneRoad.GetEndPosition().position;
    }

    public Vector3 GetLaneStart()
    {
        return laneRoad.GetStartPosition().position;
    }

    public void SetLampLight(int state)
    {
        if (lampostManager == null)
        {
            Debug.Log("lampostManager is null");
            this.lampostManager = GetComponentInChildren<LampostManager>();
        }
        else
        {
            Debug.Log("lamplight" + lampostManager.name);
            lampostManager.SetLight(state);

        }
    }

    

}