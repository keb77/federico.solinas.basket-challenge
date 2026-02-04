using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private PlayQuitUI playQuitUI;
    [SerializeField] private SelectGameModeUI selectGameModeUI;
    [SerializeField] private SelectDifficultyUI selectDifficultyUI;

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