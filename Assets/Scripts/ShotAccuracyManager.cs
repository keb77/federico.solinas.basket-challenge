using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShotAccuracy
    {
        Perfect,
        RingShort,
        RingLong,
        MissShort,
        MissLong,
        Backboard
    }

public class ShotAccuracyManager : MonoBehaviour
{
    public static ShotAccuracyManager Instance { get; private set; }

    [SerializeField] private float minDistancePerfectShotPower = 0.3f;
    [SerializeField] private float maxDistancePerfectShotPower = 0.7f;
    [SerializeField] private float perfectShotRadius = 0.03f;
    [SerializeField] private float ringShotRadius = 0.6f;
    [SerializeField] private float backboardShotOffset = 0.2f;
    [SerializeField] private float backboardShotRadius = 0.02f;

    private void Awake()
    {
        Instance = this;
    }

    public ShotAccuracy DetermineShotAccuracy(float shotPower, Transform ball, Transform perfectTarget)
    {
        float perfectShotPower = GetPerfectShotPower(ball, perfectTarget);
        float backboardShotPower = perfectShotPower + backboardShotOffset;

        float perfectPowerDelta = Mathf.Abs(shotPower - perfectShotPower);
        float backboardPowerDelta = Mathf.Abs(shotPower - backboardShotPower);

        if (perfectPowerDelta <= perfectShotRadius)
        {
            return ShotAccuracy.Perfect;
        }
        else if (perfectPowerDelta <= ringShotRadius)
        {
            return shotPower < perfectShotPower ? ShotAccuracy.RingShort : ShotAccuracy.RingLong;
        }
        else if (backboardPowerDelta <= backboardShotRadius)
        {
            return ShotAccuracy.Backboard;
        }
        else
        {
            return shotPower < perfectShotPower ? ShotAccuracy.MissShort : ShotAccuracy.MissLong;
        }
    }

    public float GetPerfectShotPower(Transform ball, Transform perfectTarget)
    {
        Vector2 ballPosition2D = new Vector2(ball.position.x, transform.position.z);
        Vector2 hoopPosition2D = new Vector2(perfectTarget.position.x, perfectTarget.position.z);
        float distanceFromHoop = Vector2.Distance(ballPosition2D, hoopPosition2D);

        float minDistanceFromHoop = ShotPositionManager.Instance.GetMinDistanceFromHoop();
        float maxDistanceFromHoop = ShotPositionManager.Instance.GetMaxDistanceFromHoop();

        float t = Mathf.InverseLerp(minDistanceFromHoop, maxDistanceFromHoop, distanceFromHoop);
        return Mathf.Lerp(minDistancePerfectShotPower, maxDistancePerfectShotPower, t);
    }
    
    public float GetBackboardShotOffset()
    {
        return backboardShotOffset;
    }
    public float GetPerfectShotRadius()
    {
        return perfectShotRadius;
    }
    public float GetBackboardShotRadius()
    {
        return backboardShotRadius;
    }
}
