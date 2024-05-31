using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GloballaneManager : MonoBehaviour
{
    public PreBuildCorssPoints BuildCorssPoints;

    public float LaneScalar = 1;
    public bool LoopSpawn;

    // Start is called before the first frame update
    public GameObject Traffic;

    public GameObject CarLanePrefab;

    public GameObject BikeLanePrefab;

    public GameObject WalkLanePrefab;

    public GameObject PrebuiltLaneA;
    public GameObject PrebuiltLaneB;
    public GameObject PrebuiltLaneE;
    public GameObject PrebuiltLaneF;

    public float OntruimingsTijdAuto = 20;
    public float OntruimingsTijdFietser = 20;
    public float OntruimingsTijdLoper = 20;

    public int timeInSec;
    public float updateTimer;
    public List<GameObject> Carlanes;
    public List<GameObject> CarlanesB;
    public List<GameObject> BikeLanes;
    public List<GameObject> BikeLanesB;

    public List<GameObject> WalkLanes;
    public List<GameObject> WalkLanesB;

    public int SpawnMin, SpawnMax;

    public List<GameObject> Voetpaden;
    public List<GameObject> TrafficList = new List<GameObject>();

    public List<CarSensormsg> carSensormsgsA = new List<CarSensormsg>();
    public List<SingleDetector> BikelaneA = new List<SingleDetector>();
    public List<SingleDetector> WalklaneA = new List<SingleDetector>();

    public List<CarSensormsg> carSensormsgsB = new List<CarSensormsg>();
    public List<SingleDetector> BikelaneB = new List<SingleDetector>();
    public List<SingleDetector> WalklaneB = new List<SingleDetector>();

    public List<CarSensormsg> carSensormsgsC = new List<CarSensormsg>();

    public List<CarSensormsg> carSensormsgsD = new List<CarSensormsg>();

    public List<CarSensormsg> carSensormsgsE = new List<CarSensormsg>();
    public List<SingleDetector> BikelaneE = new List<SingleDetector>();
    public List<SingleDetector> WalklaneE = new List<SingleDetector>();

    public List<CarSensormsg> carSensormsgsF = new List<CarSensormsg>();
    public List<SingleDetector> BikelaneF = new List<SingleDetector>();
    public List<SingleDetector> WalklaneF = new List<SingleDetector>();

    //called by the client upon the first frame

    public SignalGroup SignalGroup = new SignalGroup();
    private LaneLampSettr LaneLampSettr = new LaneLampSettr();

    public Clientbetter Communicator;

    private void Start()
    {
        this.LaneLampSettr.SetupSettr(this);
        StartCoroutine(getLane(timeInSec));
        StartCoroutine(Simutick(timeInSec));
        StartCoroutine(UpdatePakket(updateTimer));
    }

    public void SetOntruimingsTijdAuto(float tijd)
    {
        this.OntruimingsTijdAuto = tijd;
    }

    public void SetOntruimingsTijdFietser(float tijd)
    {
        this.OntruimingsTijdFietser = tijd;
    }

    public void SetOntruimingsTijdLoper(float tijd)
    {
        this.OntruimingsTijdLoper = tijd;
    }

    private IEnumerator getLane(int timeInSec)
    {
        //refresh simulation state 5 secs
        yield return new WaitForSeconds(timeInSec);
        getdata();
        getdataFromPreBuildB();
        getBikeLaneInfo();

        this.SignalGroup = GetSignalGroup();
    }

    private IEnumerator UpdatePakket(float ticktime)
    {
        yield return new WaitForSeconds(ticktime);
        string japsie = JsonConvert.SerializeObject(SignalGroup);
        if (Communicator != null)
        {
            Communicator.jsonjapp = japsie;
        }

        //Debug.Log(japsie);
        StartCoroutine(UpdatePakket(ticktime));
    }

    //time based loop for refreshing important things
    private IEnumerator Simutick(int timeInSec)
    {
        //refresh simulation state 5 secs
        yield return new WaitForSeconds(timeInSec);
        SpawnRandomCars(SpawnMin, SpawnMax);
        if (LoopSpawn)
        {
            StartCoroutine(Simutick(timeInSec));
        }
    }

    //use this to assign cars to lights
    private void SpawnRandomCars(int min, int max)
    {
        foreach (GameObject go in Carlanes)
        {
            int c = UnityEngine.Random.Range(min, max);
            CreateCars(go.GetComponentInChildren<CarLanebehaviour>(), c);
        }
        foreach (GameObject go in CarlanesB)
        {
            int c = UnityEngine.Random.Range(min, max);
            CreateCars(go.GetComponentInChildren<CarLanebehaviour>(), c);
        }
        foreach (GameObject go in WalkLanes)
        {
            int c = UnityEngine.Random.Range(min, max);
            CreateWalkers(go.GetComponentInChildren<WalkLanebehaviour>(), c);
        }
        foreach (GameObject go in WalkLanesB)
        {
            int c = UnityEngine.Random.Range(min, max);
            CreateWalkers(go.GetComponentInChildren<WalkLanebehaviour>(), c);
        }
        foreach (GameObject go in BikeLanes)
        {
            int c = UnityEngine.Random.Range(min, max);
            CreateBikers(go.GetComponentInChildren<FietsLaanBehaviour>(), c);
        }
        foreach (GameObject go in BikeLanesB)
        {
            int c = UnityEngine.Random.Range(min, max);
            CreateBikers(go.GetComponentInChildren<FietsLaanBehaviour>(), c);
        }
    }

    //public List<SingleDetector> GetSingleDetectors(GameObject prebuiltBlock, bool getBikers)
    //{
    //    List<SingleDetector> singleDetectors = new List<SingleDetector>();

    //    IEnumerable<GameObject> lanes = getBikers
    //        ? prebuiltBlock.GetComponent<PrebuildBlockInfo>().GetBikeLanes()
    //        : prebuiltBlock.GetComponent<PrebuildBlockInfo>().GetPedestrianLanes();

    //    foreach (GameObject lane in lanes)
    //    {
    //        SingleDetector singleDetector = lane?.GetComponent<SingleDetector>();
    //        if (singleDetector != null)
    //        {
    //            singleDetectors.Add(singleDetector);
    //        }
    //    }

    //    return singleDetectors;
    //}

    //public List<SingleDetector> GetSingleDetectors (GameObject PrebuiltBlock, bool GetBikers)
    //{
    //    if (GetBikers)
    //    {
    //        return PrebuiltBlock.GetComponent<PrebuildBlockInfo>()
    //                            .GetBikeLanes()
    //                            .Select(selector: obj => obj.GetComponent<SingleDetector>())
    //                            .Where(detector => detector != null)
    //                            .ToList();

    //    }

    //    return PrebuiltBlock.GetComponent<PrebuildBlockInfo>()
    //                .GetPedestrianLanes()
    //                .Select(selector: obj => obj?.GetComponent<SingleDetector>())
    //                .Where(detector => detector != null)
    //                .ToList();

    //}

    public void getBikeLaneInfo()
    {
        //PrebuiltLaneA.GetComponent<PrebuildBlockInfo>().fietsLaanBehaviours
        WalklaneA = PrebuiltLaneA.GetComponent<PrebuildBlockInfo>().WalklaneScripts;
        WalklaneB = PrebuiltLaneB.GetComponent<PrebuildBlockInfo>().WalklaneScripts;
        WalklaneE = PrebuiltLaneE.GetComponent<PrebuildBlockInfo>().WalklaneScripts;
        WalklaneF = PrebuiltLaneF.GetComponent<PrebuildBlockInfo>().WalklaneScripts;

        BikelaneA = PrebuiltLaneA.GetComponent<PrebuildBlockInfo>().BikelaneScripts;
        BikelaneB = PrebuiltLaneB.GetComponent<PrebuildBlockInfo>().BikelaneScripts;
        BikelaneE = PrebuiltLaneE.GetComponent<PrebuildBlockInfo>().BikelaneScripts;
        BikelaneF = PrebuiltLaneF.GetComponent<PrebuildBlockInfo>().BikelaneScripts;

        ExtractObjects();
    }

    private void ExtractObjects()
    {
        BikeLanes.AddRange(PrebuiltLaneA.GetComponent<PrebuildBlockInfo>().BikeLanes);
        BikeLanes.AddRange(PrebuiltLaneB.GetComponent<PrebuildBlockInfo>().BikeLanes);

        BikeLanesB.AddRange(PrebuiltLaneE.GetComponent<PrebuildBlockInfo>().BikeLanes);
        BikeLanesB.AddRange(PrebuiltLaneF.GetComponent<PrebuildBlockInfo>().BikeLanes);

        WalkLanes.AddRange(PrebuiltLaneA.GetComponent<PrebuildBlockInfo>().PedestrianLanes);
        WalkLanes.AddRange(PrebuiltLaneB.GetComponent<PrebuildBlockInfo>().PedestrianLanes);

        WalkLanesB.AddRange(PrebuiltLaneE.GetComponent<PrebuildBlockInfo>().PedestrianLanes);
        WalkLanesB.AddRange(PrebuiltLaneF.GetComponent<PrebuildBlockInfo>().PedestrianLanes);
    }

    private void getdata()
    {
        //for abc
        for (int i = 0; i < 4; i++)
        {
            carSensormsgsA.Add(Carlanes[i].GetComponentInChildren<CarLanebehaviour>().DetectorLus);
        }

        for (int i = 4; i < 4 + 4; i++)
        {
            carSensormsgsB.Add(Carlanes[i].GetComponentInChildren<CarLanebehaviour>().DetectorLus);
        }
        for (int i = 4 + 4; i < 4 + 4 + 4; i++)
        {
            carSensormsgsC.Add(Carlanes[i].GetComponentInChildren<CarLanebehaviour>().DetectorLus);
        }
    }

    private void getdataFromPreBuildB()
    {
        //for Def
        for (int i = 0; i < 4; i++)
        {
            carSensormsgsD.Add(CarlanesB[i].GetComponentInChildren<CarLanebehaviour>().DetectorLus);
        }

        for (int i = 4; i < 4 + 3; i++)
        {
            carSensormsgsE.Add(CarlanesB[i].GetComponentInChildren<CarLanebehaviour>().DetectorLus);
        }
        for (int i = 4 + 3; i < 4 + 3 + 4; i++)
        {
            carSensormsgsF.Add(CarlanesB[i].GetComponentInChildren<CarLanebehaviour>().DetectorLus);
        }
    }

    //create and add cars to the light
    private void CreateCars(CarLanebehaviour go, int count)
    {
        if (go.IsMiddlePoint)
        {
            return;
        }

        for (int i = 0; i < count; i++)
        {
            GameObject kees = Instantiate(CarActorCollection.GetRandomCarPrefab(), go.GetSpwanPos(), Quaternion.identity);
            kees.GetComponentInChildren<ActorPathFinding>().Setroute(go.GetLaneStart(), go.GetLaneStartSignal(), go.GetLaneExit());
            kees.GetComponentInChildren<ActorPathFinding>().watch = go.LampostManager.watch;
            kees.GetComponentInChildren<Movement>().Setup();
            kees.GetComponentInChildren<Movement>().SetNewDuration(OntruimingsTijdAuto);
        }
    }

    private void CreateBus(CarLanebehaviour go)
    {
        GameObject kees = Instantiate(CarActorCollection.GetBussprefab(), go.GetSpwanPos(), Quaternion.identity);
        kees.GetComponentInChildren<ActorPathFinding>().Setroute(go.GetLaneStart(), go.GetLaneStartSignal(), go.GetLaneExit());
        kees.GetComponentInChildren<ActorPathFinding>().watch = go.LampostManager.watch;
        kees.GetComponentInChildren<Movement>().Setup();
        kees.GetComponentInChildren<Movement>().SetNewDuration(OntruimingsTijdAuto);
    }

    private void CreatePrioVeh(CarLanebehaviour go)
    {
        GameObject kees = Instantiate(CarActorCollection.GetPrioPrefab(), go.GetSpwanPos(), Quaternion.identity);
        kees.GetComponentInChildren<ActorPathFinding>().Setroute(go.GetLaneStart(), go.GetLaneStartSignal(), go.GetLaneExit());
        kees.GetComponentInChildren<ActorPathFinding>().watch = go.LampostManager.watch;
        kees.GetComponentInChildren<Movement>().Setup();
        kees.GetComponentInChildren<Movement>().SetNewDuration(OntruimingsTijdAuto);
    }

    private void CreateWalkers(WalkLanebehaviour go, int count)
    {
        if (go.IsMiddlePoint)
        {
            return;
        }

        if (go.lampostManager == null)
        {
            go.lampostManager = GetComponentInChildren<LampostManager>();
            //return;
        }

        if (go.lampostManager == null)
        {
            return;
        }
        for (int i = 0; i < count; i++)
        {
            GameObject kees = Instantiate(CarActorCollection.GetPedestrianPrefab(), go.GetLaneStart(), Quaternion.identity);
            kees.GetComponent<ActorPathFinding>().Setroute(go.GetLaneStart(), go.GetLaneStartSignal(), go.GetLaneExit());
            kees.GetComponent<ActorPathFinding>().watch = go.lampostManager.watch;

            kees.GetComponent<Movement>().Setup();
            kees.GetComponent<Movement>().SetNewDuration(OntruimingsTijdLoper);
        }
    }

    private void CreateBikers(FietsLaanBehaviour go, int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject kees = Instantiate(CarActorCollection.GetBikerPrefab(), go.GetLaneStart(), Quaternion.identity);
            kees.GetComponent<ActorPathFinding>().Setroute(go.GetLaneStart(), go.GetLaneStartSignal(), go.GetLaneExit());
            kees.GetComponent<ActorPathFinding>().watch = go.LampostManager.watch;
            kees.GetComponent<Movement>().Setup();
            kees.GetComponent<Movement>().SetNewDuration(OntruimingsTijdFietser);
        }
    }

    public SignalGroup GetSignalGroup()
    {
        SignalGroup group = new SignalGroup();

        group.blocksMsg1 = AssignMsgData();
        group.blocksMsg2 = AssignMsg2Data();

        string japsie = JsonConvert.SerializeObject(group);

        Debug.Log(japsie);
        return group;
    }

    public blocksMsg AssignMsgData()
    {
        blocksMsg msg = new blocksMsg();
        msg.A = AssignblockmsgData(carSensormsgsA, CarActorCollection.Singlesfa, CarActorCollection.Singleswa);
        msg.B = AssignblockmsgBusData(carSensormsgsB, CarActorCollection.Singlesfa, CarActorCollection.Singleswa, PrebuiltLaneB.GetComponent<PrebuildBlockInfo>().BBusnr);
        msg.C = AssignblocksmsgCarOnlyData(carSensormsgsC);

        return msg;
    }

    public blockmsg AssignblockmsgData(List<CarSensormsg> carSensormsgs, List<SingleDetector> bikers, List<SingleDetector> walks)
    {
        blockmsg msg = new blockmsg();
        msg.LCarSensormsgs = carSensormsgs;
        msg.Bikers = bikers;
        msg.Walkers = walks;

        return msg;
    }

    public blockmsgBus AssignblockmsgBusData(List<CarSensormsg> carSensormsgs, List<SingleDetector> bikers, List<SingleDetector> walks, int nr)
    {
        blockmsgBus msgBus = new blockmsgBus();

        msgBus.LCarSensormsgs = carSensormsgs;
        msgBus.Walkers = walks;

        msgBus.Bikers = bikers;
        msgBus.LBusses = new List<int> { nr };
        return msgBus;
    }

    //returns caronly spots
    public blockmsgCarOnly AssignblocksmsgCarOnlyData(List<CarSensormsg> carSensormsgs)
    {
        blockmsgCarOnly msgCarOnly = new blockmsgCarOnly();
        msgCarOnly.LCarSensormsgs = carSensormsgs;
        return msgCarOnly;
    }

    public blocksMsg2 AssignMsg2Data()
    {
        blocksMsg2 msg = new blocksMsg2();
        msg.D = AssignblocksmsgCarOnlyData(carSensormsgsD);
        msg.E = AssignblockmsgBusData(carSensormsgsE, CarActorCollection.Singlesfa, CarActorCollection.Singleswa, PrebuiltLaneE.GetComponent<PrebuildBlockInfo>().BBusnr);
        msg.F = AssignblockmsgData(carSensormsgsF, CarActorCollection.Singlesfa, CarActorCollection.Singleswa);
        return msg;
    }

    public void Update()
    {
        if (Communicator.updatedKees)
        {
            LaneLampSettr.DecodeJappie(Communicator.kees);
            Communicator.updatedKees = false;
        }
    }
}