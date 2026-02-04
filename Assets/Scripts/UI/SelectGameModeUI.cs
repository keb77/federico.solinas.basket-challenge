using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectGameModeUI : MonoBehaviour
{
    [SerializeField] private MainMenuUI mainMenuUI;
    [SerializeField] private Button challengeButton;
    [SerializeField] private Button practiceButton;
    [SerializeField] private Button backButton;

    private void Awake()
    {
        challengeButton.onClick.AddListener(() =>
        {
            GameSettings.Instance.CurrentGameMode = GameMode.Challenge;
            mainMenuUI.ShowSelectDifficulty();
        });
        practiceButton.onClick.AddListener(() =>
        {
            GameSettings.Instance.CurrentGameMode = GameMode.Practice;
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
