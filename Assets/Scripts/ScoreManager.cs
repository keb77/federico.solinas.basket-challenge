using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public int Score { get; private set; }

    private void Awake()
    {
        Instance = this;

        Score = 0;
    }

    private bool ballEnteredTop = false;
    public bool HasScored { get; private set; }

    public void OnTopTriggerEnter()
    {
        if (HasScored) return;

        ballEnteredTop = true;
    }

    public void OnBottomTriggerExit()
    {
        if (HasScored) return;

        if (ballEnteredTop)
        {
            ScoreManager.Instance.AddScore();
            HasScored = true;
        }
    }

    public void ResetScoreTrigger()
    {
        ballEnteredTop = false;
        HasScored = false;
    }

    private void AddScore()
    {
        PlayerShooter.ShotAccuracy accuracy = PlayerShooter.Instance.LastShotAccuracy;

        switch (accuracy)
        {
            case PlayerShooter.ShotAccuracy.Perfect:
                Score += 3;
                break;
            case PlayerShooter.ShotAccuracy.RingShort:
            case PlayerShooter.ShotAccuracy.RingLong:
            case PlayerShooter.ShotAccuracy.Backboard:
                Score += 2;
                break;
            default:
                break;
        }
        
        Debug.Log("Score: " + Score);
    }
}
