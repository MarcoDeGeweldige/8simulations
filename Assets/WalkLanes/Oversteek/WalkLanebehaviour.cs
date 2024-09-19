using UnityEngine;

public class WalkLanebehaviour : MonoBehaviour
{
    public SingleDetector detectorLus;
    public float laneStartDistance;

    public LampostManager lampostManager;
    public bool IsMiddlePoint = false;

    private LaneCommunicator communicator;

    public RoadNode nearlus;

    // Start is called before the first frame update
    private void Start()
    {
        communicator = this.GetComponent<LaneCommunicator>();

        if(communicator.NearWaitAndDetectLus != null)
        {
            nearlus = communicator.NearWaitAndDetectLus;
        }

        if(nearlus != null)
        {
            nearlus.IsDetector = true;
            nearlus.IsSTopPoint = true;
            nearlus.LampWatch = this.lampostManager.watch;
        }
    }


    public void SetLampLight(int state)
    {
        if(lampostManager == null)
        {
            this.lampostManager = GetComponentInChildren<LampostManager>();
        }
        else
        {
            lampostManager.SetLight(state);
        }
    }
}