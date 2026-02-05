using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FireballBarUI : MonoBehaviour
{
    [SerializeField] private FireballHandler playerFireballHandler;
    [SerializeField] private Image barFillImage;
    [SerializeField] private Image barBackgroundImage;
    [SerializeField] private Image multiplierImage;
    [SerializeField] private TextMeshProUGUI multiplierText;
    [SerializeField] private Color unactiveColor;
    [SerializeField] private Color activeColor;

    private void Start()
    {
        multiplierText.text = "x" + playerFireballHandler.GetBonusMultiplier().ToString();
    }
    
    private void Update()
    {
        float fillAmount = playerFireballHandler.GetCurrentCharge() / playerFireballHandler.GetBonusActivationThreshold();
        barFillImage.fillAmount = fillAmount;

        if (playerFireballHandler.IsBonusActive())
        {
            barBackgroundImage.color = activeColor;
            multiplierImage.color = activeColor;
        }
        else
        {
            barBackgroundImage.color = unactiveColor;
            multiplierImage.color = unactiveColor;
        }
    }
}
