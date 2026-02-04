using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotEndDetector : MonoBehaviour
{
    [SerializeField] private PlayerShooter playerShooter;
    [SerializeField] private AIShooter aiShooter;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerBall"))
        {
            playerShooter.OnShotEnd(ScoreManager.Instance.HasScored(ShooterType.Player));
        }
        else if (other.CompareTag("AIBall"))
        {
            aiShooter.OnShotEnd(ScoreManager.Instance.HasScored(ShooterType.AI));
        }
    }
}
