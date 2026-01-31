using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerShooter : MonoBehaviour
{
    public static PlayerShooter Instance { get; private set; }

    [Header("References")]
    [SerializeField] private GameObject ball;
    [SerializeField] private Transform PerfectTarget;
    [SerializeField] private Transform BackboardTarget;
    private Rigidbody ballRb;

    [Header("Shot Settings")]
    public float minDistanceFromHoop = 6.0f;
    public float maxDistanceFromHoop = 10.0f;
    public float spinSpeed = 30f;

    [Header("Accuracy Settings")]
    public float minDistancePerfectShotPower = 0.3f;
    public float maxDistancePerfectShotPower = 0.7f;
    public float perfectShotRadius = 0.025f;
    public float ringShotRadius = 0.05f;
    public float backboardOffset = 0.2f;
    public float backboardShotRadius = 0.01f;

    public ShotAccuracy LastShotAccuracy { get; private set; }

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
        ResetBall();
        ScoreManager.Instance.ResetScoreTrigger();

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
        float timeToTarget = 1.5f;
        Vector3 noise = Vector3.zero;

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
                timeToTarget = 1.2f;
                break;
        }

        Vector3 velocity = (targetPosition + noise - startPosition) / timeToTarget - 0.5f * gravity * timeToTarget;
        return velocity;
    }

    private void ResetBall()
    {
        ballRb.useGravity = false;
        ballRb.velocity = Vector3.zero;
        ballRb.angularVelocity = Vector3.zero;
        ball.transform.position = transform.position;
    }
}
