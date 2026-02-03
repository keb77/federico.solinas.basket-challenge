using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public int Score { get; private set; }

    [SerializeField] private int perfectShotScore = 3;
    [SerializeField] private int regularShotScore = 2;
    [SerializeField] private ScoreFlyerGeneratorUI scoreFlyerGeneratorUI;

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
        int scoreToAdd = 0;

        if (PlayerShooter.Instance.IsPerfectShot())
        {
            scoreToAdd = perfectShotScore;
        }
        else
        {
            if (BackboardBonusManager.Instance.IsBonusActive && PlayerShooter.Instance.IsBackboardShot())
            {
                scoreToAdd += regularShotScore + BackboardBonusManager.Instance.CurrentBonusPoints;
                BackboardBonusManager.Instance.ResetBonus();
            }
            else
            {
                scoreToAdd += regularShotScore;
            }
        }

        Score += scoreToAdd;
        scoreFlyerGeneratorUI.CreateScoreFlyer(scoreToAdd);
    }
}
