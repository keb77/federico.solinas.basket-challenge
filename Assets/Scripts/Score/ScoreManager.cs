using System.Collections.Generic;
using UnityEngine;

/// Manages player and AI scores.
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private PlayerShooter playerShooter;
    [SerializeField] private AIShooter aiShooter;
    [SerializeField] private BackboardBonusManager backboardBonusManager;
    [SerializeField] private ScoreEffectPlayer scoreEffectPlayer;

    [Header("Score Settings")]
    [SerializeField] private int perfectShotScore = 3;
    [SerializeField] private int regularShotScore = 2;


    // Data structure to track score and shot state for each shooter
    private class ShooterScoreData
    {
        public int Score = 0;
        public bool BallEnteredTop = false; // Tracks if the ball entered the top trigger for the current shot
        public bool HasScored = false; // Tracks if the shooter has already scored for the current shot
    }
    private Dictionary<ShooterType, ShooterScoreData> shooterData = new Dictionary<ShooterType, ShooterScoreData>();

    private void OnValidate()
    {
        if (playerShooter == null || aiShooter == null || backboardBonusManager == null || scoreEffectPlayer == null)
        {
            Debug.LogWarning("ScoreManager: Some fields are not assigned.", this);
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Initialize score data
        shooterData[ShooterType.Player] = new ShooterScoreData();
        shooterData[ShooterType.AI] = new ShooterScoreData();
    }

    /// Called when a ball enters the top trigger.
    public void OnTopTriggerEnter(ShooterType shooterType)
    {
        ShooterScoreData data = shooterData[shooterType];
        if (data.HasScored) return;
        data.BallEnteredTop = true;
    }

    /// Called when a ball exits the bottom trigger. Adds score if the ball entered the top trigger and hasn't scored yet.
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

        // Perfect shot
        if (shooter.IsPerfectShot())
        {
            scoreToAdd = perfectShotScore;
        }
        else
        {
            // Backboard bonus
            if (backboardBonusManager != null && backboardBonusManager.IsBonusActive && shooter.IsBackboardShot())
            {
                scoreToAdd += regularShotScore + backboardBonusManager.CurrentBonusPoints;
                backboardBonusManager.ResetBonus();
            }
            // Regular shot
            else
            {
                scoreToAdd += regularShotScore;
            }
        }

        // Fireball bonus
        FireballHandler fireballHandler = shooter.GetFireballHandler();
        if (fireballHandler != null && fireballHandler.IsBonusActive())
        {
            scoreToAdd *= fireballHandler.GetBonusMultiplier();
        }

        data.Score += scoreToAdd;

        // Play score effects for player
        if (shooterType == ShooterType.Player && scoreEffectPlayer != null)
        {
            scoreEffectPlayer.PlayScoreEffects(scoreToAdd);
        }
    }

    public int GetScore(ShooterType shooterType) => shooterData[shooterType].Score;
    public bool HasScored(ShooterType shooterType) => shooterData[shooterType].HasScored;
}
