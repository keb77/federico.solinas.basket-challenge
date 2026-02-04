using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private TextMeshProUGUI playerScoreText;
    [SerializeField] private TextMeshProUGUI aiScoreText;
    [SerializeField] private Button playAgainButton;
    [SerializeField] private Button mainMenuButton;

    private void Awake()
    {
        playAgainButton.onClick.AddListener(() => {
            SceneManager.LoadScene(1);
        });

        mainMenuButton.onClick.AddListener(() => {
            SceneManager.LoadScene(0);
        });
    }
    
    private void Start()
    {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;

        Hide();
    }

    private void GameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsGameOver())
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        int playerScore = ScoreManager.Instance.GetScore(ShooterType.Player);
        int aiScore = ScoreManager.Instance.GetScore(ShooterType.AI);

        playerScoreText.text = "Your Score: " + playerScore.ToString();
        
        GameMode gameMode = GameSettings.Instance != null ? GameSettings.Instance.CurrentGameMode : GameMode.Practice;
        AIDifficulty aiDifficulty = GameSettings.Instance != null ? GameSettings.Instance.CurrentAIDifficulty : AIDifficulty.Rookie;
        if (gameMode == GameMode.Practice)
        {
            aiScoreText.text = "";
        }
        else
        {
            switch (GameSettings.Instance.CurrentAIDifficulty)
            {
                case AIDifficulty.Rookie:
                    aiScoreText.text = "Rookie Rick's Score: " + aiScore.ToString();
                    break;
                case AIDifficulty.Pro:
                    aiScoreText.text = "Pro Pete's Score: " + aiScore.ToString();
                    break;
                case AIDifficulty.AllStar:
                    aiScoreText.text = "All-Star Ace's Score: " + aiScore.ToString();
                    break;
            }
        }

        if (gameMode == GameMode.Practice)
        {
            gameOverText.text = "GAME OVER";
        }
        else if (playerScore > aiScore)
        {
            gameOverText.text = "YOU WIN";
        }
        else if (playerScore < aiScore)
        {
            gameOverText.text = "YOU LOSE";
        }
        else
        {
            gameOverText.text = "IT'S A TIE";
        }
        
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
