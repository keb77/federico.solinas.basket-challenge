using UnityEngine;
using UnityEngine.UI;

/// Handles the Select Difficulty UI in the main menu.
public class SelectDifficultyUI : MonoBehaviour
{
    [SerializeField] private MainMenuUI mainMenuUI;
    [SerializeField] private Button rookieButton;
    [SerializeField] private Button proButton;
    [SerializeField] private Button allStarButton;
    [SerializeField] private Button backButton;

    private void OnValidate()
    {
        if (mainMenuUI == null || rookieButton == null || proButton == null || allStarButton == null || backButton == null)
        {
            Debug.LogWarning("SelectDifficultyUI: Some fields are not assigned.", this);
        }
    }

    private void Awake()
    {
        // Set up button listeners
        rookieButton.onClick.AddListener(() =>
        {
            if (GameSettings.Instance == null)
            {
                Debug.LogError("GameSetting instance not found.");
            }
            else
            {
                GameSettings.Instance.CurrentAIDifficulty = AIDifficulty.Rookie;
            }

            mainMenuUI.StartGame();
        });
        proButton.onClick.AddListener(() =>
        {
            if (GameSettings.Instance == null)
            {
                Debug.LogError("GameSetting instance not found.");
            }
            else
            {
                GameSettings.Instance.CurrentAIDifficulty = AIDifficulty.Pro;
            }

            mainMenuUI.StartGame();
        });
        allStarButton.onClick.AddListener(() =>
        {
            if (GameSettings.Instance == null)
            {
                Debug.LogError("GameSetting instance not found.");
            }
            else
            {
                GameSettings.Instance.CurrentAIDifficulty = AIDifficulty.AllStar;
            }

            mainMenuUI.StartGame();
        });
        backButton.onClick.AddListener(() =>
        {
            mainMenuUI.ShowSelectGameMode();
        });
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
