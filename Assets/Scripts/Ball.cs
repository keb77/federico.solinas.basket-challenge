using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{   
    private Rigidbody rb;

    public enum ShotAccuracy
    {
        Perfect,
        RingShort,
        RingLong,
        MissShort,
        MissLong,
        Backboard
    }

    [SerializeField] private Transform PerfectTarget;
    [SerializeField] private Transform BackboardTarget;

    public void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    public Vector3 CalculateShotVelocity(ShotAccuracy accuracy)
    {
        // Projectile motion equation: p(t) = p0 ​+ v0 ​t + 1/2 ​g t^2
        // => v_0 = (p(t) - p_0) / t - 1/2 g t
        
        Vector3 startPosition = transform.position;
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
                noise = (targetPosition - startPosition).normalized * Random.Range(-1.5f, -1f);
                break;
            case ShotAccuracy.MissLong:
                targetPosition = PerfectTarget.position;
                noise = (targetPosition - startPosition).normalized * Random.Range(1f, 1.5f);
                break;
            case ShotAccuracy.Backboard:
                targetPosition = BackboardTarget.position;
                timeToTarget = 1.2f;
                break;
        }

        Vector3 velocity = (targetPosition + noise - startPosition) / timeToTarget - 0.5f * gravity * timeToTarget;
        return velocity;
    }

    public void Shoot(Vector3 velocity)
    {
        rb.useGravity = true;
        rb.velocity = velocity;

        Vector3 spinAxis = Vector3.Cross(velocity.normalized, Vector3.up); 
        float spinSpeed = 30f;
        rb.angularVelocity = spinAxis * spinSpeed;
    }

    public void Start()
    {
        Vector3 shotVelocity = CalculateShotVelocity(ShotAccuracy.Backboard);
        Shoot(shotVelocity);
    }
}
