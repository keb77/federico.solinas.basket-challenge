using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreTrigger : MonoBehaviour
{
    public enum TriggerZone
    {
        Top,
        Bottom
    }

    [SerializeField] private TriggerZone triggerZone;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("PlayerBall"))
        {
            if (triggerZone == TriggerZone.Top)
            {
                ScoreManager.Instance.OnTopTriggerEnter(ShooterType.Player);
            }
        }
        else if (collision.CompareTag("AIBall"))
        {
            if (triggerZone == TriggerZone.Top)
            {
                ScoreManager.Instance.OnTopTriggerEnter(ShooterType.AI);
            }
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("PlayerBall"))
        {
            if (triggerZone == TriggerZone.Bottom)
            {
                ScoreManager.Instance.OnBottomTriggerExit(ShooterType.Player);
            }
        }
        else if (collision.CompareTag("AIBall"))
        {
            if (triggerZone == TriggerZone.Bottom)
            {
                ScoreManager.Instance.OnBottomTriggerExit(ShooterType.AI);
            }
        }
    }
}
