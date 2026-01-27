using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
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
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
