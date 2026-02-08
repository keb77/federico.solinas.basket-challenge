using UnityEngine;

/// Detects when a ball passes through scoring trigger zones (top or bottom) and notifies the ScoreManager accordingly. 
public class ScoreTrigger : MonoBehaviour
{
    public enum TriggerZone
    {
        Top,
        Bottom
    }

    [Tooltip("Specify whether this is the top or bottom score trigger.")]
    [SerializeField] private TriggerZone triggerZone;
    [SerializeField] private string playerBallTag = "PlayerBall";
    [SerializeField] private string aiBallTag = "AIBall";

    private void OnTriggerEnter(Collider collision)
    {
        if (ScoreManager.Instance == null)
        {
            Debug.LogWarning("ScoreManager instance not found.");
            return;
        }

        if (collision.CompareTag(playerBallTag))
        {
            if (triggerZone == TriggerZone.Top)
            {
                ScoreManager.Instance.OnTopTriggerEnter(ShooterType.Player);
            }
        }
        else if (collision.CompareTag(aiBallTag))
        {
            if (triggerZone == TriggerZone.Top)
            {
                ScoreManager.Instance.OnTopTriggerEnter(ShooterType.AI);
            }
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (ScoreManager.Instance == null)
        {
            Debug.LogWarning("ScoreManager instance not found.");
            return;
        }

        if (collision.CompareTag(playerBallTag))
        {
            if (triggerZone == TriggerZone.Bottom)
            {
                ScoreManager.Instance.OnBottomTriggerExit(ShooterType.Player);
            }
        }
        else if (collision.CompareTag(aiBallTag))
        {
            if (triggerZone == TriggerZone.Bottom)
            {
                ScoreManager.Instance.OnBottomTriggerExit(ShooterType.AI);
            }
        }
    }
}
