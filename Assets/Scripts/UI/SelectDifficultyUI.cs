using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectDifficultyUI : MonoBehaviour
{
    [SerializeField] private MainMenuUI mainMenuUI;
    [SerializeField] private Button rookieButton;
    [SerializeField] private Button proButton;
    [SerializeField] private Button allStarButton;
    [SerializeField] private Button backButton;

    private void Awake()
    {
        rookieButton.onClick.AddListener(() =>
        {
            GameSettings.Instance.CurrentAIDifficulty = AIDifficulty.Rookie;
            mainMenuUI.StartGame();
        });
        proButton.onClick.AddListener(() =>
        {
            GameSettings.Instance.CurrentAIDifficulty = AIDifficulty.Pro;
            mainMenuUI.StartGame();
        });
        allStarButton.onClick.AddListener(() =>
        {
            GameSettings.Instance.CurrentAIDifficulty = AIDifficulty.AllStar;
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
