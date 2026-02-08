using System.Collections.Generic;
using UnityEngine;

/// Stores global game settings (game mode and AI difficulty).
public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance { get; private set; }

    public GameMode CurrentGameMode { get; set; } = GameMode.Practice;
    public AIDifficulty CurrentAIDifficulty { get; set; } = AIDifficulty.Rookie;

    public Dictionary<AIDifficulty, string> AIDifficultyNames = new Dictionary<AIDifficulty, string>
    {
        { AIDifficulty.Rookie, "Rick" },
        { AIDifficulty.Pro, "Pete" },
        { AIDifficulty.AllStar, "Ace" }
    };

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Persist across scenes
        DontDestroyOnLoad(gameObject);
    }
}

public enum GameMode
{
    Challenge,
    Practice
}

public enum AIDifficulty
{
    Rookie,
    Pro,
    AllStar
}