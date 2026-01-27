using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountdownToStartUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;

    private void Start()
    {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;

        Hide();
    }

    private void GameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsCountdownToStartActive())
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
    

    private void Update()
    {
        if (GameManager.Instance.IsCountdownToStartActive())
        {
            float countdownToStartTimer = GameManager.Instance.GetCountdownToStartTimer();
            countdownText.text = Mathf.Ceil(countdownToStartTimer).ToString();
        }
    }
}
