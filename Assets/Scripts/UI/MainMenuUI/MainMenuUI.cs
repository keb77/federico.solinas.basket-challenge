using UnityEngine;
using UnityEngine.SceneManagement;

/// Handles the main menu UI navigation, starting the game, and quitting the application.
public class MainMenuUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private PlayQuitUI playQuitUI;
    [SerializeField] private SelectGameModeUI selectGameModeUI;
    [SerializeField] private SelectDifficultyUI selectDifficultyUI;

    private void OnValidate()
    {
        if (playQuitUI == null || selectGameModeUI == null || selectDifficultyUI == null)
        {
            Debug.LogWarning("MainMenuUI: Some fields are not assigned.", this);
        }
    }

    private void Start()
    {
        ShowPlayQuit();
    }

    public void ShowPlayQuit()
    {
        playQuitUI.Show();
        selectGameModeUI.Hide();
        selectDifficultyUI.Hide();
    }
    public void ShowSelectGameMode()
    {
        selectGameModeUI.Show();
        playQuitUI.Hide();
        selectDifficultyUI.Hide();
    }
    public void ShowSelectDifficulty()
    {
        selectDifficultyUI.Show();
        playQuitUI.Hide();
        selectGameModeUI.Hide();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}