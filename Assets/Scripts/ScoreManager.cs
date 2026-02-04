using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private PlayerShooter playerShooter;
    [SerializeField] private AIShooter aiShooter;
    [SerializeField] private ScoreFlyerGeneratorUI scoreFlyerGeneratorUI;

    [Header("Score Settings")]
    [SerializeField] private int perfectShotScore = 3;
    [SerializeField] private int regularShotScore = 2;

    private class ShooterScoreData
    {
        public int Score;
        public bool BallEnteredTop;
        public bool HasScored;
    }
    private Dictionary<ShooterType, ShooterScoreData> shooterData = new Dictionary<ShooterType, ShooterScoreData>();

    private void Awake()
    {
        Instance = this;

        shooterData[ShooterType.Player] = new ShooterScoreData();
        shooterData[ShooterType.AI] = new ShooterScoreData();
    }

    public void OnTopTriggerEnter(ShooterType shooterType)
    {
        ShooterScoreData data = shooterData[shooterType];
        if (data.HasScored) return;
        data.BallEnteredTop = true;
    }

    public void OnBottomTriggerExit(ShooterType shooterType)
    {
        ShooterScoreData data = shooterData[shooterType];
        if (data.HasScored) return;
        if (data.BallEnteredTop)
        {
            AddScore(shooterType);
            data.HasScored = true;
        }
    }

    public void ResetScoreTrigger(ShooterType shooterType)
    {
        ShooterScoreData data = shooterData[shooterType];
        data.BallEnteredTop = false;
        data.HasScored = false;
    }

    private void AddScore(ShooterType shooterType)
    {
        Shooter shooter = shooterType == ShooterType.Player ? (Shooter)playerShooter : (Shooter)aiShooter;
        ShooterScoreData data = shooterData[shooterType];

        int scoreToAdd = 0;

        if (shooter.IsPerfectShot())
        {
            scoreToAdd = perfectShotScore;
        }
        else
        {
            if (BackboardBonusManager.Instance.IsBonusActive && shooter.IsBackboardShot())
            {
                scoreToAdd += regularShotScore + BackboardBonusManager.Instance.CurrentBonusPoints;
                BackboardBonusManager.Instance.ResetBonus();
            }
            else
            {
                scoreToAdd += regularShotScore;
            }
        }

        data.Score += scoreToAdd;

        if (shooterType == ShooterType.Player)
        {
            scoreFlyerGeneratorUI.CreateScoreFlyer(scoreToAdd);
        }
    }

    public int GetScore(ShooterType shooterType)
    {
        return shooterData[shooterType].Score;
    }
    public bool HasScored(ShooterType shooterType)
    {
        return shooterData[shooterType].HasScored;
    }
}
