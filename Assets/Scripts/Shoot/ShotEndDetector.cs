using UnityEngine;

/// Detects when a shot has ended by checking for collisions with the ball and notifying the appropriate shooter (player or AI) about the result of the shot (scored or missed).
public class ShotEndDetector : MonoBehaviour
{
    [SerializeField] private PlayerShooter playerShooter;
    [SerializeField] private AIShooter aiShooter;
    [SerializeField] private string playerBallTag = "PlayerBall";
    [SerializeField] private string aiBallTag = "AIBall";

    private void OnValidate()
    {
        if (playerShooter == null || aiShooter == null)
        {
            Debug.LogWarning("ShotEndDetector: Some fields are not assigned.", this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerBallTag))
        {
            playerShooter.OnShotEnd(ScoreManager.Instance.HasScored(ShooterType.Player));
        }
        else if (other.CompareTag(aiBallTag))
        {
            aiShooter.OnShotEnd(ScoreManager.Instance.HasScored(ShooterType.AI));
        }
    }
}
