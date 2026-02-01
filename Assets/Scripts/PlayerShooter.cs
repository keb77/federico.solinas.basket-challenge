using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    public static PlayerShooter Instance { get; private set; }

    [Header("References")]
    [SerializeField] private GameObject ball;
    [SerializeField] private Transform PerfectTarget;
    [SerializeField] private Transform BackboardTarget;
    private Rigidbody ballRb;

    [Header("Shot Position Settings")]
    [SerializeField] private float minDistanceFromHoop = 6.0f;
    [SerializeField] private float maxDistanceFromHoop = 10.0f;
    [SerializeField] private float maxAngleFromHoop = 45f;

    [Header("Shot Settings")]
    [SerializeField] private float timeToTarget = 1.5f;
    [SerializeField] private float timeToTargetBackboard = 1.2f;
    [SerializeField] private float spinSpeed = 30f;

    [Header("Accuracy Settings")]
    [SerializeField] private float minDistancePerfectShotPower = 0.3f;
    [SerializeField] private float maxDistancePerfectShotPower = 0.7f;
    [SerializeField] private float perfectShotRadius = 0.03f;
    [SerializeField] private float ringShotRadius = 0.05f;
    [SerializeField] private float backboardOffset = 0.2f;
    [SerializeField] private float backboardShotRadius = 0.02f;

    public ShotAccuracy LastShotAccuracy { get; private set; }
    
    public bool IsBallInPlay { get;  private set; } = false;

    private void Awake()
    {
        Instance = this;

        ballRb = ball.GetComponent<Rigidbody>();
        ResetBall();
    }

    public enum ShotAccuracy
    {
        Perfect,
        RingShort,
        RingLong,
        MissShort,
        MissLong,
        Backboard
    }

    public void Shoot(float shotPower)
    {
        InputManager.Instance.CanShoot = false;
        IsBallInPlay = true;

        CameraManager.Instance.SetCameraFollowingBall();

        ShotAccuracy accuracy = DetermineShotAccuracy(shotPower);
        Vector3 velocity = CalculateShotVelocity(accuracy);

        ballRb.useGravity = true;
        ballRb.velocity = velocity;

        Vector3 spinAxis = Vector3.Cross(velocity.normalized, Vector3.up);
        ballRb.angularVelocity = spinAxis * spinSpeed;

        LastShotAccuracy = accuracy;
    }

    private ShotAccuracy DetermineShotAccuracy(float shotPower)
    {
        Vector2 ballPosition2D = new Vector2(transform.position.x, transform.position.z);
        Vector2 hoopPosition2D = new Vector2(PerfectTarget.position.x, PerfectTarget.position.z);
        float distanceFromHoop = Vector2.Distance(ballPosition2D, hoopPosition2D);

        float t = Mathf.InverseLerp(minDistanceFromHoop, maxDistanceFromHoop, distanceFromHoop);
        float perfectShotPower = Mathf.Lerp(minDistancePerfectShotPower, maxDistancePerfectShotPower, t);
        float backboardShotPower = perfectShotPower + backboardOffset;

        float perfectPowerDelta = Mathf.Abs(shotPower - perfectShotPower);
        float backboardPowerDelta = Mathf.Abs(shotPower - backboardShotPower);

        Debug.Log($"Shot Power: {shotPower:F2}, Perfect Shot Delta: {perfectPowerDelta:F2}, Backboard Shot Delta: {backboardPowerDelta:F2}");

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
            return shotPower < backboardShotPower ? ShotAccuracy.MissShort : ShotAccuracy.MissLong;
        }
    }

    private Vector3 CalculateShotVelocity(ShotAccuracy accuracy)
    {
        // Projectile motion equation: p(t) = p0 ​+ v0 ​t + 1/2 ​g t^2
        // => v_0 = (p(t) - p_0) / t - 1/2 g t

        Vector3 startPosition = ball.transform.position;
        Vector3 targetPosition = Vector3.zero;
        Vector3 gravity = Physics.gravity;
        Vector3 noise = Vector3.zero;
        float finalTimeToTarget = timeToTarget;

        switch (accuracy)
        {
            case ShotAccuracy.Perfect:
                targetPosition = PerfectTarget.position;
                break;
            case ShotAccuracy.RingShort:
                targetPosition = PerfectTarget.position;
                noise = (targetPosition - startPosition).normalized * Random.Range(-0.4f, -0.2f);
                break;
            case ShotAccuracy.RingLong:
                targetPosition = PerfectTarget.position;
                noise = (targetPosition - startPosition).normalized * Random.Range(0.2f, 0.4f);
                break;
            case ShotAccuracy.MissShort:
                targetPosition = PerfectTarget.position;
                noise = (targetPosition - startPosition).normalized * -1.5f;
                break;
            case ShotAccuracy.MissLong:
                targetPosition = PerfectTarget.position;
                noise = (targetPosition - startPosition).normalized * 1.5f;
                break;
            case ShotAccuracy.Backboard:
                targetPosition = BackboardTarget.position;
                finalTimeToTarget = timeToTargetBackboard;
                break;
        }

        Vector3 velocity = (targetPosition + noise - startPosition) / finalTimeToTarget - 0.5f * gravity * finalTimeToTarget;
        return velocity;
    }

    private void ResetBall()
    {
        ballRb.useGravity = false;
        ballRb.velocity = Vector3.zero;
        ballRb.angularVelocity = Vector3.zero;
        ball.transform.position = transform.position;

        IsBallInPlay = false;
    }

    public void OnShotEnd(bool hasScored)
    {
        if (hasScored)
        {
            MovePlayerToRandomPosition();
        }

        ResetBall();
        CameraManager.Instance.SetCameraBehindPlayer();
        ScoreManager.Instance.ResetScoreTrigger();
        if (GameManager.Instance.IsGamePlaying())
        {
            InputManager.Instance.CanShoot = true;
        }
    }

    private void MovePlayerToRandomPosition()
    {
        Vector3 newPosition = GenerateRandomPosition();
        transform.parent.position = newPosition;

        Vector3 newDirection = PerfectTarget.position - newPosition;
        newDirection.y = 0f;
        transform.parent.rotation = Quaternion.LookRotation(newDirection);
    }

    private Vector3 GenerateRandomPosition()
    {
        float distance = Random.Range(minDistanceFromHoop, maxDistanceFromHoop);
        float angle = Random.Range(-maxAngleFromHoop, maxAngleFromHoop);
        Vector3 direction = Quaternion.Euler(0, angle, 0) * PerfectTarget.forward;
        Vector3 position = PerfectTarget.position + direction * distance;
        position.y = 0f;
        return position;
    }
}
