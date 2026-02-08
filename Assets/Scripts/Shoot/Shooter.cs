using UnityEngine;

public enum ShooterType
{
    Player,
    AI
}

/// Base class for handling shooting mechanics for both player and AI shooters.
public class Shooter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected GameObject ball;
    [SerializeField] protected Transform perfectTarget;
    [SerializeField] private Transform backboardTarget;
    [SerializeField] protected ShotPositionManager shotPositionManager;
    [SerializeField] protected ShotAccuracyManager shotAccuracyManager;
    [SerializeField] protected BackboardBonusManager backboardBonusManager;
    [SerializeField] private FireballHandler fireballHandler;
    [SerializeField] private Animator shooterAnimator;

    [Header("Shot Settings")]
    [Tooltip("Time it takes for the ball to reach the target on a perfect shot.")]
    [SerializeField] private float timeToTarget = 1.5f;
    [Tooltip("Time it takes for the ball to reach the target on a backboard shot.")]
    [SerializeField] private float timeToTargetBackboard = 1.25f;
    [Tooltip("Spin speed applied to the ball for visual effect.")]
    [SerializeField] private float spinSpeed = 30f;

    private Rigidbody ballRb;
    private ShotAccuracy lastShotAccuracy;
    private bool isBallInPlay = false;

    public ShooterType ShooterType { get; protected set; }

    private void OnValidate()
    {
        if (ball == null || perfectTarget == null || backboardTarget == null || shotPositionManager == null || shotAccuracyManager == null ||
            backboardBonusManager == null || fireballHandler == null || shooterAnimator == null)
        {
            Debug.LogWarning("Shooter: Some fields are not assigned.", this);
        }
    }

    protected virtual void Awake()
    {
        ballRb = ball.GetComponent<Rigidbody>();
        
        ResetBall();
    }

    public virtual void Shoot(float shotPower)
    {
        if (isBallInPlay) return;
        isBallInPlay = true;

        // Trigger shooting animation
        shooterAnimator.SetTrigger("Shoot");

        // Determine shot accuracy and calculate velocity
        ShotAccuracy accuracy = shotAccuracyManager.DetermineShotAccuracy(shotPower, transform, perfectTarget);
        Vector3 velocity = CalculateShotVelocity(accuracy);

        // Apply velocity and spin to the ball
        ballRb.useGravity = true;
        ballRb.velocity = velocity;
        Vector3 spinAxis = Vector3.Cross(velocity.normalized, Vector3.up);
        ballRb.angularVelocity = spinAxis * spinSpeed;

        lastShotAccuracy = accuracy;
    }

    // Calculates the initial velocity needed for the ball to reach the target based on the shot accuracy.
    // Projectile motion equation: p(t) = p0 ​+ v0 ​t + 1/2 ​g t^2
    // => v_0 = (p(t) - p_0) / t - 1/2 g t
    protected Vector3 CalculateShotVelocity(ShotAccuracy accuracy)
    {
        Vector3 startPosition = ball.transform.position;
        Vector3 targetPosition = Vector3.zero;
        Vector3 gravity = Physics.gravity;
        Vector3 noise = Vector3.zero;
        float finalTimeToTarget = timeToTarget;

        // Adjust target position and time to target based on shot accuracy
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
                finalTimeToTarget = timeToTargetBackboard;
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
        // Move the player to a new random position if they scored, otherwise just reset the ball
        if (hasScored)
        {
            MoveToRandomPosition();
            fireballHandler.OnBasketScored();
        }
        else
        {
            fireballHandler.OnBasketMissed();
        }

        ResetBall();

        // Reset the score trigger for this shooter type in the ScoreManager
        if (ScoreManager.Instance == null)
        {
            Debug.LogWarning("ScoreManager instance not found.");
            return;
        }
        ScoreManager.Instance.ResetScoreTrigger(ShooterType);
    }

    private void MoveToRandomPosition()
    {
        Vector3 newPosition = shotPositionManager.GenerateRandomPosition(perfectTarget);
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
    
    public bool IsBallInPlay() => isBallInPlay;
    public bool IsPerfectShot() => lastShotAccuracy == ShotAccuracy.Perfect;
    public bool IsBackboardShot() => lastShotAccuracy == ShotAccuracy.Backboard;
    public FireballHandler GetFireballHandler() => fireballHandler;
}
