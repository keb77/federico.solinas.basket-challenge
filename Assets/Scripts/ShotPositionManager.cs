using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotPositionManager : MonoBehaviour
{
    public static ShotPositionManager Instance { get; private set; }

    [SerializeField] private float minDistanceFromHoop = 6.0f;
    [SerializeField] private float maxDistanceFromHoop = 10.0f;
    [SerializeField] private float maxAngleFromHoop = 45f;

    private void Awake()
    {
        Instance = this;
    }

    public Vector3 GenerateRandomPosition(Transform hoop)
    {
        float distance = Random.Range(minDistanceFromHoop, maxDistanceFromHoop);
        float angle = Random.Range(-maxAngleFromHoop, maxAngleFromHoop);
        Vector3 direction = Quaternion.Euler(0, angle, 0) * hoop.forward;
        Vector3 position = hoop.position + direction * distance;
        position.y = 0f;
        return position;
    }

    public float GetMinDistanceFromHoop()
    {
        return minDistanceFromHoop;
    }
    public float GetMaxDistanceFromHoop()
    {
        return maxDistanceFromHoop;
    }
}
