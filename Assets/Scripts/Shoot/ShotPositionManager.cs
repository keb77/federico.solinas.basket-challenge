using UnityEngine;

/// Manages the generation of random shot positions around the hoop for the player to shoot from.
public class ShotPositionManager : MonoBehaviour
{
    [Tooltip("Minimum distance from the hoop for generated shot positions.")]
    [SerializeField] private float minDistanceFromHoop = 6.0f;
    [Tooltip("Maximum distance from the hoop for generated shot positions.")]
    [SerializeField] private float maxDistanceFromHoop = 10.0f;
    [Tooltip("Maximum angle (in degrees) from the hoop's forward direction for generated shot positions.")]
    [SerializeField] private float maxAngleFromHoop = 45f;

    public Vector3 GenerateRandomPosition(Transform hoop)
    {
        float distance = Random.Range(minDistanceFromHoop, maxDistanceFromHoop);
        float angle = Random.Range(-maxAngleFromHoop, maxAngleFromHoop);
        Vector3 direction = Quaternion.Euler(0, angle, 0) * hoop.forward;
        Vector3 position = hoop.position + direction * distance;
        position.y = 0f;
        return position;
    }

    public float GetMinDistanceFromHoop() => minDistanceFromHoop;
    public float GetMaxDistanceFromHoop() => maxDistanceFromHoop;
}
