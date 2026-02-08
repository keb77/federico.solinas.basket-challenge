using UnityEngine;
using UnityEngine.UI;

/// Handles the Play/Quit UI in the main menu.
public class PlayQuitUI : MonoBehaviour
{
    [SerializeField] private MainMenuUI mainMenuUI;
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    private void OnValidate()
    {
        if (mainMenuUI == null || playButton == null || quitButton == null)
        {
            Debug.LogWarning("PlayQuitUI: Some fields are not assigned.", this);
        }
    }

    private void Awake()
    {
        // Set up button listeners
        playButton.onClick.AddListener(() =>
        {
            mainMenuUI.ShowSelectGameMode();
        });
        quitButton.onClick.AddListener(() =>
        {
            mainMenuUI.QuitGame();
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
