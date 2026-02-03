using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayingTimerUI : MonoBehaviour
{
    [SerializeField] private Image clockFillImage;

    private void Update()
    {
        if (GameManager.Instance.IsGamePlaying())
        {
            clockFillImage.fillAmount = GameManager.Instance.GetGamePlayingTimerNormalized();
        }
    }
}
