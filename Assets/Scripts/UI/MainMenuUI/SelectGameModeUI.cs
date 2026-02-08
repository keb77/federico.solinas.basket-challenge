using UnityEngine;
using UnityEngine.UI;

/// Handles the Select Game Mode UI in the main menu.
public class SelectGameModeUI : MonoBehaviour
{
    [SerializeField] private MainMenuUI mainMenuUI;
    [SerializeField] private Button challengeButton;
    [SerializeField] private Button practiceButton;
    [SerializeField] private Button backButton;

    private void OnValidate()
    {
        if (mainMenuUI == null || challengeButton == null || practiceButton == null || backButton == null)
        {
            Debug.LogWarning("SelectGameModeUI: Some fields are not assigned.", this);
        }
    }

    private void Awake()
    {
        // Set up button listeners
        challengeButton.onClick.AddListener(() =>
        {
            if (GameSettings.Instance == null)
            {
                Debug.LogError("GameSetting instance not found.");
            }
            else
            {
                GameSettings.Instance.CurrentGameMode = GameMode.Challenge;
            }
            
            mainMenuUI.ShowSelectDifficulty();
        });
        practiceButton.onClick.AddListener(() =>
        {
            if (GameSettings.Instance == null)
            {
                Debug.LogError("GameSetting instance not found.");
            }
            else
            {
                GameSettings.Instance.CurrentGameMode = GameMode.Practice;
            }
            
            mainMenuUI.StartGame();
        });
        backButton.onClick.AddListener(() =>
        {
            mainMenuUI.ShowPlayQuit();
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
