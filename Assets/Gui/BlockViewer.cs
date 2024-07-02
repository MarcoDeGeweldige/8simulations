
using UnityEngine;
using UnityEngine.Events;

public class BlockViewer : MonoBehaviour
{
    // Rotation speed and maximum threshold
    public float rotationSpeed;
    public float maxThresl;

    // The target GameObject
    public GameObject Target;

    // Toggle input flag
    public bool ToggledInput = false;

    // Current focus index
    private int currentfocus = 1;

    // Blocks A to F
    public GameObject BlockA;
    public GameObject BlockB;
    public GameObject BlockC;
    public GameObject BlockD;
    public GameObject BlockE;
    public GameObject BlockF;

    // Camera offset
    public Vector3 Camoffset;

    // Main camera
    public Camera Cam;

    // Unity event for block switching
    public UnityEvent<string> OnBlockSwitch;

    private void Start()
    {
        // Initialize camera position and focus
        Cam = Camera.main;
        Cam.transform.position = BlockA.transform.position;
        currentfocus = 1;
        Target = getCurrentFocus();
    }

    private GameObject getCurrentFocus()
    {
        // Determine the current focused block based on index
        switch(currentfocus)
        {
            case 1:
                OnBlockSwitch.Invoke("A");
                return BlockA;

            case 2:
                OnBlockSwitch.Invoke("B");
                return BlockB;

            case 3:
                OnBlockSwitch.Invoke("C");
                return BlockC;

            case 4:
                OnBlockSwitch.Invoke("D");
                return BlockD;

            case 5:
                OnBlockSwitch.Invoke("E");
                return BlockE;

            case 6:
                OnBlockSwitch.Invoke("F");
                return BlockF;

            default:
                currentfocus = 1;
                OnBlockSwitch.Invoke("A");
                return BlockA;
        }
    }

    private void LateUpdate()
    {
        // Switch focus on Space key press
        if(Input.GetKeyDown(KeyCode.Space))
        {
            currentfocus++;
            Target = getCurrentFocus();
        }

        Transform target = Target.transform;

        // Update camera position and rotation
        this.transform.position = target.position - target.forward * Camoffset.z + target.up * Camoffset.y;
        transform.LookAt(target);
    }
}
