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

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
