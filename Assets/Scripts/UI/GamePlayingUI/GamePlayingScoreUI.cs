using UnityEngine;
using TMPro;

/// Handles the score UI during the game.
public class GamePlayingScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerScoreText;
    [SerializeField] private TextMeshProUGUI aiScoreText;

    private void OnValidate()
    {
        if (playerScoreText == null || aiScoreText == null)
        {
            Debug.LogWarning("GamePlayingScoreUI: Some fields are not assigned.", this);
        }
    }

    private void Start()
    {
        if (GameManager.Instance == null || GameSettings.Instance == null)
        {
            Debug.LogWarning("GameManager or GameSettings instance not found.");
        }
    }

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
