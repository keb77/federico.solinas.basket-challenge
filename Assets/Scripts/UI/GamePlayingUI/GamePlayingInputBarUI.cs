using UnityEngine;
using UnityEngine.UI;

/// Displays the input bar that shows the player's swipe input power and the target zones for perfect and backboard shots during the game playing state.
public class GamePlayingInputBarUI : MonoBehaviour
{
    [SerializeField] private Transform playerShooter;
    [SerializeField] private Transform perfectTarget;
    [SerializeField] private ShotAccuracyManager playerShotAccuracyManager;

    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image barFillImage;
    [SerializeField] private Image perfectShotZoneImage;
    [SerializeField] private Image backboardShotZoneImage;

    private float backgroundImageHeight;

    private void OnValidate()
    {
        if (playerShooter == null || perfectTarget == null || playerShotAccuracyManager == null ||
            backgroundImage == null || barFillImage == null || perfectShotZoneImage == null || backboardShotZoneImage == null)
        {
            Debug.LogWarning("GamePlayingInputBarUI: Some fields are not assigned.", this);
        }
    }

    private void Start()
    {
        backgroundImageHeight = backgroundImage.rectTransform.rect.height;

        SetTargetZones();

        if (GameManager.Instance == null || InputManager.Instance == null)
        {
            Debug.LogWarning("GameManager or InputManager instance not found.");
        }
    }

    private void Update()
    {
        if (GameManager.Instance.IsGamePlaying())
        {
            // Update the fill amount of the input bar based on the player's current swipe input power
            barFillImage.fillAmount = InputManager.Instance.GetCurrentSwipeMaxDistanceNormalized();

            SetTargetZones();
        }
    }

    /// Sets the positions and sizes of the perfect shot and backboard shot target zones
    private void SetTargetZones()
    {
        float perfectShotPower = playerShotAccuracyManager.GetPerfectShotPower(playerShooter, perfectTarget);
        float backboardShotPower = perfectShotPower + playerShotAccuracyManager.GetBackboardShotOffset();
        float perfectShotRadius = playerShotAccuracyManager.GetPerfectShotRadius();
        float backboardShotRadius = playerShotAccuracyManager.GetBackboardShotRadius();

        // Set the center of the target zones based on the height of the input bar
        float perfectShotZoneY = Mathf.Clamp01(perfectShotPower) * backgroundImageHeight;
        float backboardShotZoneY = Mathf.Clamp01(backboardShotPower) * backgroundImageHeight;
        perfectShotZoneImage.rectTransform.anchoredPosition = new Vector2(
            perfectShotZoneImage.rectTransform.anchoredPosition.x,
            perfectShotZoneY - backgroundImageHeight / 2f
        );
        backboardShotZoneImage.rectTransform.anchoredPosition = new Vector2(
            backboardShotZoneImage.rectTransform.anchoredPosition.x,
            backboardShotZoneY - backgroundImageHeight / 2f
        );

        // Set the heights of the target zones
        float perfectShotZoneHeight = Mathf.Clamp01(perfectShotRadius) * backgroundImageHeight * 2f;
        float backboardShotZoneHeight = Mathf.Clamp01(backboardShotRadius) * backgroundImageHeight * 2f;
        perfectShotZoneImage.rectTransform.sizeDelta = new Vector2(
            perfectShotZoneImage.rectTransform.sizeDelta.x,
            perfectShotZoneHeight
        );
        backboardShotZoneImage.rectTransform.sizeDelta = new Vector2(
            backboardShotZoneImage.rectTransform.sizeDelta.x,
            backboardShotZoneHeight
        );
    }
}
