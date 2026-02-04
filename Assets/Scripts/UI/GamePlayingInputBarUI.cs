using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayingInputBarUI : MonoBehaviour
{
    [SerializeField] private Transform playerShooter;
    [SerializeField] private Transform perfectTarget;

    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image barFillImage;
    [SerializeField] private Image perfectShotZoneImage;
    [SerializeField] private Image backboardShotZoneImage;

    private float backgroundImageHeight;

    private void Start()
    {
        backgroundImageHeight = backgroundImage.rectTransform.rect.height;
    }

    private void Update()
    {
        if (GameManager.Instance.IsGamePlaying())
        {
            barFillImage.fillAmount = InputManager.Instance.GetCurrentSwipeMaxDistanceNormalized();

            float perfectShotPower = ShotAccuracyManager.Instance.GetPerfectShotPower(playerShooter, perfectTarget);
            float backboardShotPower = perfectShotPower + ShotAccuracyManager.Instance.GetBackboardShotOffset();
            float perfectShotRadius = ShotAccuracyManager.Instance.GetPerfectShotRadius();
            float backboardShotRadius = ShotAccuracyManager.Instance.GetBackboardShotRadius();

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
}
