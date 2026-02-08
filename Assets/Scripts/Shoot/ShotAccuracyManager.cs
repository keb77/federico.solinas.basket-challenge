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

/// Manages the accuracy of shots based on the shot power and the position of the ball relative to the hoop.
public class ShotAccuracyManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ShotPositionManager shotPositionManager;

    [Header("Shot Accuracy Settings")]
    [Tooltip("The shot power required for a perfect shot at the closest distance to the hoop.")]
    [SerializeField] private float minDistancePerfectShotPower = 0.3f;
    [Tooltip("The shot power required for a perfect shot at the farthest distance from the hoop.")]
    [SerializeField] private float maxDistancePerfectShotPower = 0.7f;
    [Tooltip("The tolerance around the perfect shot power that counts as a perfect shot.")]
    [SerializeField] private float perfectShotRadius = 0.03f;
    [Tooltip("The tolerance around the perfect shot power that counts as a ring shot.")]
    [SerializeField] private float ringShotRadius = 0.06f;
    [Tooltip("The difference in shot power between a perfect shot and a backboard shot.")]
    [SerializeField] private float backboardShotOffset = 0.2f;
    [Tooltip("The tolerance around the backboard shot power that counts as a backboard shot.")]
    [SerializeField] private float backboardShotRadius = 0.02f;

    private void OnValidate()
    {
        if (shotPositionManager == null)
        {
            Debug.LogWarning("ShotAccuracyManager: Some fields are not assigned.", this);
        }
    }

    public ShotAccuracy DetermineShotAccuracy(float shotPower, Transform ball, Transform perfectTarget)
    {
        // Calculate the shot power for a perfect shot and a backboard shot
        float perfectShotPower = GetPerfectShotPower(ball, perfectTarget);
        float backboardShotPower = perfectShotPower + backboardShotOffset;

        // Calculate the difference between the shot power and the perfect shot power and the backboard shot power
        float perfectPowerDelta = Mathf.Abs(shotPower - perfectShotPower);
        float backboardPowerDelta = Mathf.Abs(shotPower - backboardShotPower);

        // Determine the shot accuracy based on the tolerance thresholds
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
        // Calculate the distance from the ball to the hoop in the horizontal plane
        Vector2 ballPosition2D = new Vector2(ball.position.x, transform.position.z);
        Vector2 hoopPosition2D = new Vector2(perfectTarget.position.x, perfectTarget.position.z);
        float distanceFromHoop = Vector2.Distance(ballPosition2D, hoopPosition2D);

        // Get the minimum and maximum distances from the hoop from the ShotPositionManager
        float minDistanceFromHoop = shotPositionManager.GetMinDistanceFromHoop();
        float maxDistanceFromHoop = shotPositionManager.GetMaxDistanceFromHoop();

        // Find and return the shot power for a perfect shot based on the distance from the hoop and the defined range for positions
        float t = Mathf.InverseLerp(minDistanceFromHoop, maxDistanceFromHoop, distanceFromHoop);
        return Mathf.Lerp(minDistancePerfectShotPower, maxDistancePerfectShotPower, t);
    }

    public float GetBackboardShotOffset() => backboardShotOffset;
    public float GetPerfectShotRadius() => perfectShotRadius;
    public float GetBackboardShotRadius() => backboardShotRadius;

}
