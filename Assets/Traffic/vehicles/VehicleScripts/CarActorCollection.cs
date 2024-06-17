using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages a collection of car-related prefabs and detectors.
/// </summary>
public class CarActorCollection : MonoBehaviour
{
    // Serialized field for car prefabs
    [SerializeField]
    private List<GameObject> carPrefabs = new List<GameObject>();

    [SerializeField]
    private List<GameObject> PedPrefabs = new List<GameObject>();

    [SerializeField]
    private List<GameObject> BikePrefabs = new List<GameObject>();

    // Private field for priority vehicle prefab
    private GameObject PrioVehiclePrefab;

    [SerializeField]
    private GameObject CarPrefab;

    // Serialized fields for other prefabs
    [SerializeField]
    private GameObject busPrefab;

    [SerializeField]
    private GameObject PedestrianPrefab;

    [SerializeField]
    private GameObject bikerPrefab;

    // Static fields for storing prefab references
    private static GameObject pedestrianStaticPrefab;

    public static GameObject staticBikerPrefab;
    private static GameObject staticbusPrefab;
    private static GameObject staticPrioVehiclePrefab;

    private static GameObject staticRealCarPrefab;
    public static List<GameObject> StaticCarPrefabs { get; private set; }
    public static List<GameObject> StaticPedPrefabs { get; private set; }
    public static List<GameObject> StaticBikePrefabs { get; private set; }

    // Lists for storing detectors
    public List<SingleDetector> Singlews { get; private set; }

    public static List<SingleDetector> Singleswa { get; private set; }
    public static List<SingleDetector> Singlesfa { get; private set; }
    public static List<SingleDetector> Singleswb { get; private set; }
    public static List<SingleDetector> Singlesfb { get; private set; }

    // Awake method initializes static fields and lists
    private void Awake()
    {
        StaticCarPrefabs = new List<GameObject>(carPrefabs);
        StaticBikePrefabs = new List<GameObject>(BikePrefabs);
        StaticPedPrefabs = new List<GameObject>(PedPrefabs);
        Singlesfa = new List<SingleDetector>();
        Singleswb = new List<SingleDetector>();
        Singlesfb = new List<SingleDetector>();
        Singleswa = new List<SingleDetector>();
        pedestrianStaticPrefab = PedestrianPrefab;
        staticbusPrefab = busPrefab;
        staticPrioVehiclePrefab = PrioVehiclePrefab;
        staticBikerPrefab = bikerPrefab;
        staticRealCarPrefab = CarPrefab;
    }

    // Static method to get a random car prefab from the list
    public static GameObject GetRandomCarPrefab()
    {
        if (StaticCarPrefabs.Count == 0)
        {
            Debug.LogError("No car prefabs available.");
            return null;
        }
        int randomIndex = Random.Range(0, StaticCarPrefabs.Count);
        return StaticCarPrefabs[randomIndex];
    }

    public static GameObject GetRandomBikePrefab()
    {
        if (StaticBikePrefabs.Count == 0)
        {
            Debug.LogError("No Bike prefabs available.");
            return null;
        }
        int randomIndex = Random.Range(0, StaticBikePrefabs.Count);
        return StaticBikePrefabs[randomIndex];
    }

    public static GameObject GetRandomPedPrefab()
    {
        if (StaticPedPrefabs.Count == 0)
        {
            Debug.LogError("No Ped prefabs available.");
            return null;
        }
        int randomIndex = Random.Range(0, StaticPedPrefabs.Count);
        return StaticPedPrefabs[randomIndex];
    }

    public static GameObject getCarPrefab()
    {
        return staticRealCarPrefab;
    }

    // Static methods to get specific prefabs
    public static GameObject GetBussprefab() => staticbusPrefab;

    public static GameObject GetPrioPrefab() => staticPrioVehiclePrefab;

    public static GameObject GetPedestrianPrefab() => pedestrianStaticPrefab;

    public static GameObject GetBikerPrefab() => staticBikerPrefab;

    // Static methods to add detectors to specific lists
    //public static void AddDet(SingleDetector singleDetector) => Singleswa.Add(singleDetector);

    //public static void AddDetToWB(SingleDetector singleDetector) => Singleswb.Add(singleDetector);

    //public static void AddDetToFA(SingleDetector singleDetector) => Singlesfa.Add(singleDetector);

    //public static void AddDetoFB(SingleDetector singleDetector) => Singlesfb.Add(singleDetector);
}