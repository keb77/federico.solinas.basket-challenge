using UnityEngine;
using UnityEngine.UI;

/// Displays a clock UI element that un-fills as the game playing timer counts down.
public class GamePlayingTimerUI : MonoBehaviour
{
    [SerializeField] private Image clockFillImage;

    private void OnValidate()
    {
        if (clockFillImage == null)
        {
            Debug.LogWarning("GamePlayingTimerUI: Some fields are not assigned.", this);
        }
    }

    private void Start()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogWarning("GameManager instance not found.");
        }
    }

    private void Update()
    {
        if (GameManager.Instance.IsGamePlaying())
        {
            clockFillImage.fillAmount = GameManager.Instance.GetGamePlayingTimerNormalized();
        }
    }
}
