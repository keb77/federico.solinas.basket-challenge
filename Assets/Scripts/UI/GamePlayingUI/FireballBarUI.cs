using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// Handles the fireball bar UI, which shows the player's current fireball charge and bonus multiplier. 
public class FireballBarUI : MonoBehaviour
{
    [SerializeField] private FireballHandler playerFireballHandler;
    [SerializeField] private Image barFillImage;
    [SerializeField] private Image barBackgroundImage;
    [SerializeField] private Image multiplierImage;
    [SerializeField] private TextMeshProUGUI multiplierText;
    [SerializeField] private Color unactiveColor;
    [SerializeField] private Color activeColor;

    private void OnValidate()
    {
        if (playerFireballHandler == null || barFillImage == null || barBackgroundImage == null || multiplierImage == null || multiplierText == null
            || unactiveColor == null || activeColor == null)
        {
            Debug.LogWarning("FireballBarUI: Some fields are not assigned.", this);
        }
    }

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
