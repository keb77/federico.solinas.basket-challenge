using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShooterType
{
    Player,
    AI
}

public class Shooter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected GameObject ball;
    [SerializeField] protected Transform perfectTarget;
    [SerializeField] private Transform backboardTarget;

    [Header("Shot Settings")]
    [SerializeField] private float timeToTarget = 1.5f;
    [SerializeField] private float timeToTargetBackboard = 1.35f;
    [SerializeField] private float spinSpeed = 30f;

    private Rigidbody ballRb;
    private ShotAccuracy lastShotAccuracy;
    private bool isBallInPlay = false;
    
    public ShooterType ShooterType { get; protected set; }

    protected virtual void Awake()
    {
        ballRb = ball.GetComponent<Rigidbody>();
        ResetBall();
    }

    public virtual void Shoot(float shotPower)
    {
        if (isBallInPlay) return;

        isBallInPlay = true;

        ShotAccuracy accuracy = ShotAccuracyManager.Instance.DetermineShotAccuracy(shotPower, transform, perfectTarget);
        Vector3 velocity = CalculateShotVelocity(accuracy);

        ballRb.useGravity = true;
        ballRb.velocity = velocity;

        Vector3 spinAxis = Vector3.Cross(velocity.normalized, Vector3.up);
        ballRb.angularVelocity = spinAxis * spinSpeed;

        lastShotAccuracy = accuracy;
    }

    protected Vector3 CalculateShotVelocity(ShotAccuracy accuracy)
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
                targetPosition = perfectTarget.position;
                break;
            case ShotAccuracy.RingShort:
                targetPosition = perfectTarget.position;
                noise = (targetPosition - startPosition).normalized * Random.Range(-0.4f, -0.2f);
                break;
            case ShotAccuracy.RingLong:
                targetPosition = perfectTarget.position;
                noise = (targetPosition - startPosition).normalized * Random.Range(0.2f, 0.4f);
                break;
            case ShotAccuracy.MissShort:
                targetPosition = perfectTarget.position;
                noise = (targetPosition - startPosition).normalized * -1.5f;
                break;
            case ShotAccuracy.MissLong:
                targetPosition = perfectTarget.position;
                noise = (targetPosition - startPosition).normalized * 1.5f;
                break;
            case ShotAccuracy.Backboard:
                targetPosition = backboardTarget.position;
                finalTimeToTarget = timeToTargetBackboard;
                break;
        }

        Vector3 velocity = (targetPosition + noise - startPosition) / finalTimeToTarget - 0.5f * gravity * finalTimeToTarget;
        return velocity;
    }

    public virtual void OnShotEnd(bool hasScored)
    {
        if (hasScored)
        {
            MoveToRandomPosition();
        }

        ResetBall();
        
        ScoreManager.Instance.ResetScoreTrigger(ShooterType);
    }

    private void MoveToRandomPosition()
    {
        Vector3 newPosition = ShotPositionManager.Instance.GenerateRandomPosition(perfectTarget);
        transform.parent.position = newPosition;

        Vector3 newDirection = perfectTarget.position - newPosition;
        newDirection.y = 0f;
        transform.parent.rotation = Quaternion.LookRotation(newDirection);
    }

    private void ResetBall()
    {
        ballRb.useGravity = false;
        ballRb.velocity = Vector3.zero;
        ballRb.angularVelocity = Vector3.zero;
        ball.transform.position = transform.position;

        isBallInPlay = false;
    }

    public bool IsBallInPlay()
    {
        return isBallInPlay;
    }

    public bool IsPerfectShot()
    {
        return lastShotAccuracy == ShotAccuracy.Perfect;
    }
    public bool IsBackboardShot()
    {
        return lastShotAccuracy == ShotAccuracy.Backboard;
    }
}
