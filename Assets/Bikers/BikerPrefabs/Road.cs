using UnityEngine;

/// <summary>
/// Represents a road segment with start and end positions.
/// </summary>
public class Road : MonoBehaviour
{
    [SerializeField] private Transform startPosition; // Start position of the road.
    [SerializeField] private Transform endPosition; // End position of the road.

    /// <summary>
    /// Gets the start position of the road.
    /// </summary>
    public Transform GetStartPosition()
    {
        return startPosition;
    }

    /// <summary>
    /// Gets the end position of the road.
    /// </summary>
    public Transform GetEndPosition()
    {
        return endPosition;
    }
}