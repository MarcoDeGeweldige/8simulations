using UnityEngine;

//improved version of movement
//not timebased
public class MovementM2 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private vehicleDetector vehicleDetector; // Handles vehicle detection.

    public Vector3 LookAtPosition;

    public float currentSpeed;
    public float MaxSpeed;
    public float MinSpeed;

    public float SlowDownRate = 0.1f;
    public float SpeedUpRate = 0.5f;

    public bool IsPrio = false;

    public Rigidbody body;

    private ActorNodeReader NodeReader;

    public NodeType actorNodeType = NodeType.HighSpeed;

    public Vector3 ColliderSize = new Vector3(1.8f, 1.3f, 2.04f);

    //vehicle is too close
    [SerializeField]
    private bool Detectedvehicle;

    //green light
    private bool CanGo = true;

    public LampWatch watch;

    //is the actor waiting
    public bool IsWaiting = false;

    //when driving after getting green light
    public bool PassedWaitNode = false;

    [SerializeField]
    private bool IsBus = false;

    private Vector3 targetPosition;

    public Vector3 TargetPosition
    {
        get
        {
            return targetPosition;
        }

        set
        {
            targetPosition = value;
            LookAtPosition = value;
            LookAtPosition.y = transform.position.y;
        }
    }

    private void Awake()
    {
        currentSpeed = 5;
        MaxSpeed = 40;
        MinSpeed = 0;
        this.TargetPosition = transform.position + transform.forward * 500;
        NodeReader = this.gameObject.AddComponent<ActorNodeReader>();
        NodeReader.SetActorType(actorNodeType);
        NodeReader.SetIfBus(this.IsBus);
    }

    private void Start()
    {
        if (body == null)
        {
            body = GetComponent<Rigidbody>();
        }
        this.GetComponent<BoxCollider>().size = this.ColliderSize;
        _SetVehicleDetectors();
        if (actorNodeType != NodeType.HighSpeed)
        {
            vehicleDetector.halveScanRange();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        MoveActor();
    }

    public void SetNewGoal(Vector3 _Target)
    {
        this.TargetPosition = _Target;
    }

    public void SetLampWatch(ref LampWatch watch)
    {
        if (System.Object.ReferenceEquals(null, watch))
        {
            Debug.Log("null");
            CanGo = true;
            this.PassedWaitNode = true;
            return;
        }
        if (watch.CanGo)
        {
            this.IsWaiting = false;
            this.CanGo = true;
            this.PassedWaitNode = true;
            return;
        }
        else
        {
            this.IsWaiting = true;
            this.watch = watch;
            this.PassedWaitNode = false;

            watch.OnCanGoChanged += Watch_OnCanGoChanged;
        }
    }

    public void RemoveLampWatch()
    {
        if (!System.Object.ReferenceEquals(watch, null))
        {
            watch.OnCanGoChanged -= Watch_OnCanGoChanged;
            watch = null;
            this.PassedWaitNode = false;
        }
    }

    private void MoveActor()
    {
        if (Detectedvehicle)
        {
            return;
        }
        if (this.IsWaiting)
        {
            return;
        }

        if (!CanGo)
        {
            this.IsWaiting = true;
            return;
        }

        moveActorToPos();
    }

    private void moveActorToPos()
    {
        transform.LookAt(this.LookAtPosition);

        body.MovePosition(transform.position += transform.forward * currentSpeed * Time.deltaTime);
    }

    private void _SetVehicleDetectors()
    {
        vehicleDetector = this.gameObject.AddComponent<vehicleDetector>();

        this.vehicleDetector.OnVehcleDetected += VehicleDetector_OnVehcleDetected;
        this.vehicleDetector.OnLongScanDetected += VehicleDetector_OnLongScanDetected;
    }

    private void VehicleDetector_OnLongScanDetected(bool obj)
    {
        if (obj)
        {
            if (Detectedvehicle)
            {
                currentSpeed = 0;
                return;
            }
            currentSpeed -= SlowDownRate;
            currentSpeed = Mathf.Clamp(currentSpeed, MinSpeed, MaxSpeed);
        }
        else
        {
            if (Detectedvehicle)
            {
                currentSpeed = 0;
                return;
            }
            currentSpeed += this.SpeedUpRate;
            currentSpeed = Mathf.Clamp(currentSpeed, MinSpeed, MaxSpeed);
        }
    }

    private void VehicleDetector_OnVehcleDetected(bool detected)
    {
        if (detected)
        {
            Detectedvehicle = detected;
            currentSpeed = 0;
        }
        else
        {
            Detectedvehicle = detected;
        }
    }

    private void OnDestroy()
    {
    }

    private void OnDisable()
    {
        this.vehicleDetector.OnVehcleDetected -= VehicleDetector_OnVehcleDetected;
        this.vehicleDetector.OnLongScanDetected -= VehicleDetector_OnLongScanDetected;
        this.RemoveLampWatch();
    }

    private void Watch_OnCanGoChanged(bool cango, bool hasBus)
    {
        if (IsWaiting)
        {
            if (hasBus)
            {
                if (IsBus)
                {
                    if (cango == true)
                    {
                        BusLeftLane();
                        IsWaiting = false;
                        this.CanGo = cango;
                        this.PassedWaitNode = cango;
                        RemoveLampWatch();
                    }
                    return;
                }
            }
            else
            {
                this.CanGo = cango;
                if (cango == true)
                {
                    this.IsWaiting = false;
                    this.CanGo = true;
                    this.PassedWaitNode = true;
                    RemoveLampWatch();
                    return;
                }
            }
        }
    }

    private void BusLeftLane()
    {
        watch.BussPassedLane();
    }
}