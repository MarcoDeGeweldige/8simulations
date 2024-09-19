using System;
using System.Collections;
using UnityEngine;

public class vehicleDetector : MonoBehaviour
{
    public float ScanTime = 0.1f;
    public float detectionRange = 10f; // The range of detection in front of the vehicle
    public float longScanRange = 20f;
    public LayerMask vehicleLayer; // Layer mask to filter out non-vehicle objects

    public event Action<bool> OnVehcleDetected;

    public event Action<bool> OnLongScanDetected;

    private void Start()
    {
        vehicleLayer = LayerMask.GetMask("Actor");

        // Start the coroutine when needed (e.g., when a button is clicked)
        StartCoroutine(ScanFrontl(ScanTime));
    }

    public void halveScanRange()
    {
        detectionRange *= 0.5f;
        longScanRange *= 0.5f;
    }

    private IEnumerator ScanFrontl(float time)
    {
        while(true) // Loop indefinitely
        {
            LongScanFront();
            DetectVehiclesInFront();
            yield return new WaitForSeconds(ScanTime);
        }
    }

    private void LongScanFront()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, longScanRange, vehicleLayer))
        {
            OnLongScanDetected?.Invoke(true);
        }
        else
        {
            OnLongScanDetected?.Invoke(false);
        }
    }

    private void DetectVehiclesInFront()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, detectionRange, vehicleLayer))
        {
            Debug.DrawLine(transform.position, hit.point);
            // Detected a vehicle in front
            OnVehcleDetected?.Invoke(true);
        }
        else
        {
            OnVehcleDetected?.Invoke(false);
        }
    }
}