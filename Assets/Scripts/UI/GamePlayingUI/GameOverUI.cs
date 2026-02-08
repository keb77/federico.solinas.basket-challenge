using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

/// Handles the game over UI, which is shown when the game ends. 
public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private TextMeshProUGUI playerScoreText;
    [SerializeField] private TextMeshProUGUI aiScoreText;
    [SerializeField] private Button playAgainButton;
    [SerializeField] private Button mainMenuButton;

    private void OnValidate()
    {
        if (gameOverText == null || playerScoreText == null || aiScoreText == null || playAgainButton == null || mainMenuButton == null)
        {
            Debug.LogWarning("GameOverUI: Some fields are not assigned.", this);
        }
    }

    private void Awake()
    {
        // Add listeners to the buttons
        playAgainButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(1);
        });
        mainMenuButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(0);
        });
    }

    private void Start()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogWarning("GameManager instance not found.");
            return;
        }
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
        if (ScoreManager.Instance == null)
        {
            Debug.LogWarning("ScoreManager instance not found.");
            return;
        }

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
            aiScoreText.text = GameSettings.Instance.AIDifficultyNames[aiDifficulty] + ": " + aiScore.ToString();
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
