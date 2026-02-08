using UnityEngine;
using TMPro;

/// Displays the countdown to start timer before the game starts.
public class CountdownToStartUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;

    private void OnValidate()
    {
        if (countdownText == null)
        {
            Debug.LogWarning("CountdownToStartUI: Some fields are not assigned.", this);
        }
    }

    private void Start()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager instance not found.");
            return;
        }
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;

        Hide();
    }

    private void GameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        // Show the countdown timer if the countdown to start is active, otherwise hide it.
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

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnStateChanged -= GameManager_OnStateChanged;
        }
    }
}
