using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GamePlayingScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerScoreText;
    [SerializeField] private TextMeshProUGUI aiScoreText;

    private void Update()
    {
        if (GameManager.Instance.IsGamePlaying())
        {
            int playerScore = ScoreManager.Instance.GetScore(ShooterType.Player);
            playerScoreText.text = "You: " + playerScore.ToString();

            int aiScore = ScoreManager.Instance.GetScore(ShooterType.AI);
            GameMode gameMode = GameSettings.Instance != null ? GameSettings.Instance.CurrentGameMode : GameMode.Practice;
            AIDifficulty aiDifficulty = GameSettings.Instance != null ? GameSettings.Instance.CurrentAIDifficulty : AIDifficulty.Rookie;
            if (gameMode == GameMode.Practice)
            {
                aiScoreText.text = "";
            }
            else
            {
                aiScoreText.text = GameSettings.Instance.AIDifficultyNames[aiDifficulty] + ": " + aiScore.ToString();
            }
        }
    }
}
