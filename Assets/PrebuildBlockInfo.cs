using System.Collections.Generic;
using UnityEngine;

public class PrebuildBlockInfo : MonoBehaviour
{
    public string BlockCode = "empty";
    public bool IsBlcokB = false;

    public bool HasBus = false;
    public int BBusnr = 127;

    //bike lanes
    public GameObject BikeLaneIn;

    public GameObject BikeLaneOut;

    //in block
    public GameObject PedestrianLampIn;

    public GameObject PedestriianLampMid;

    //revers block
    public GameObject PedestrianLampInR;

    public GameObject PedestriianLampMidR;

    public List<GameObject> BikeLanes = new List<GameObject>();
    public List<GameObject> PedestrianLanes = new List<GameObject>();

    public List<WalkLanebehaviour> walkLanebehaviours = new List<WalkLanebehaviour>();
    public List<FietsLaanBehaviour> fietsLaanBehaviours = new List<FietsLaanBehaviour>();

    public List<SingleDetector> BikelaneScripts = new List<SingleDetector>();
    public List<SingleDetector> WalklaneScripts = new List<SingleDetector>();

    private void Start()
    {
        buildLaneList();
        buildPedestrianLaneList();

        if (IsBlcokB)
        {
            setBblockLaneScripts(BikeLanes, true);
            setBblockLaneScripts(PedestrianLanes, false);
        }
        setablockLaneScripts(BikeLanes, true);
        setablockLaneScripts(PedestrianLanes, false);
    }

    public List<GameObject> GetPedestrianLanes() => PedestrianLanes;

    public List<GameObject> GetBikeLanes() => BikeLanes;

    public void HasBusOn(int nr)
    {
        this.HasBus = true;
        this.BBusnr = nr;
    }

    private void buildLaneList()
    {
        BikeLanes.Add(BikeLaneIn);
        fietsLaanBehaviours.Add(BikeLaneIn.GetComponentInChildren<FietsLaanBehaviour>());
        BikeLanes.Add(BikeLaneOut);
        fietsLaanBehaviours.Add(BikeLaneOut.GetComponentInChildren<FietsLaanBehaviour>());
    }

    private void buildPedestrianLaneList()
    {
        PedestrianLanes.Add(PedestrianLampIn);
        PedestrianLanes.Add(PedestriianLampMid);
        PedestrianLanes.Add(PedestriianLampMidR);
        PedestrianLanes.Add(PedestrianLampInR);
    }

    private List<SingleDetector> GetLaneScripts(List<GameObject> objects, bool isBikeLane)
    {
        List<SingleDetector> singleDetectors = new List<SingleDetector>();

        foreach (GameObject obj in objects)
        {
            if (isBikeLane)
            {
                singleDetectors.Add(obj.GetComponentInChildren<FietsLaanBehaviour>().DetectorLus);
            }
            else
            {
                singleDetectors.Add(obj.GetComponentInChildren<WalkLanebehaviour>().detectorLus);
            }
        }

        return singleDetectors;
    }

    private void setablockLaneScripts(List<GameObject> objects, bool isBikeLane)
    {
        Debug.Log(objects.Count);
        foreach (GameObject obj in objects)
        {
            if (isBikeLane)
            {
                FietsLaanBehaviour fietsLaanBehaviour = obj?.GetComponentInChildren<FietsLaanBehaviour>();
                if (fietsLaanBehaviour != null)
                {
                    CarActorCollection.AddDetToFA(fietsLaanBehaviour.DetectorLus);
                }
                //singleDetectors.Add(obj.GetComponentInChildren<FietsLaanBehaviour>().DetectorLus);
                //CarActorCollection.AddDetToFA(obj.GetComponentInChildren<FietsLaanBehaviour>().DetectorLus);
            }
            else
            {
                WalkLanebehaviour walkLaneBehaviour = obj?.GetComponentInChildren<WalkLanebehaviour>();
                if (walkLaneBehaviour != null)
                {
                    CarActorCollection.AddDet(walkLaneBehaviour.detectorLus);
                }
                //singleDetectors.Add(obj.GetComponentInChildren<WalkLanebehaviour>().detectorLus);
                //CarActorCollection.AddDet((obj.GetComponentInChildren<WalkLanebehaviour>().detectorLus));
            }
        }
    }

    private void setBblockLaneScripts(List<GameObject> objects, bool isBikeLane)
    {
        foreach (GameObject obj in objects)
        {
            if (isBikeLane)
            {
                FietsLaanBehaviour fietsLaanBehaviour = obj.GetComponentInChildren<FietsLaanBehaviour>();
                if (fietsLaanBehaviour != null)
                {
                    CarActorCollection.AddDetoFB(obj.GetComponentInChildren<FietsLaanBehaviour>().DetectorLus);
                }

                //singleDetectors.Add(obj.GetComponentInChildren<FietsLaanBehaviour>().DetectorLus);
                //CarActorCollection.AddDetoFB(obj.GetComponentInChildren<FietsLaanBehaviour>().DetectorLus);
            }
            else
            {
                WalkLanebehaviour walkLaneBehaviour = obj.GetComponentInChildren<WalkLanebehaviour>();
                if (walkLaneBehaviour != null)
                {
                    //singleDetectors.Add(obj.GetComponentInChildren<WalkLanebehaviour>().detectorLus);
                    CarActorCollection.AddDetToWB((obj.GetComponentInChildren<WalkLanebehaviour>().detectorLus));
                }
            }
        }
    }

    public void Setlamps(List<int> BikeLampsStates, List<int> PedestrianlampsStates)
    {
        for (int i = 0; i < BikeLampsStates.Count; i++)
        {
            BikeLanes[i].GetComponentInChildren<FietsLaanBehaviour>().SetLampLight(BikeLampsStates[i]);
        }
        if (PedestrianlampsStates != null && PedestrianLanes != null)
        {
            for (int i = 0; i < Mathf.Min(PedestrianlampsStates.Count, PedestrianLanes.Count - 1); i++)
            {
                var walkBehaviour = PedestrianLanes[i].GetComponentInChildren<WalkLanebehaviour>();
                if (walkBehaviour != null)
                {
                    walkBehaviour.SetLampLight(PedestrianlampsStates[i]);
                }
            }
        }
    }
}