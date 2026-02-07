using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
