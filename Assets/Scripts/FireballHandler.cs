using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballHandler : MonoBehaviour
{
    [SerializeField] private float chargePerBasket = 2f;
    [SerializeField] private float bonusActivationThreshold = 5f;
    [SerializeField] private float decaySpeedWhenInactive = 0.2f;
    [SerializeField] private float decaySpeedWhenActive = 0.4f;
    [SerializeField] private int bonusMultiplier = 2;

    private float currentCharge = 0f;
    private bool isBonusActive = false;

    private void Update()
    {
        float decaySpeed = isBonusActive ? decaySpeedWhenActive : decaySpeedWhenInactive;
        currentCharge = currentCharge - decaySpeed * Time.deltaTime;
        currentCharge = Mathf.Clamp(currentCharge, 0f, bonusActivationThreshold);
        if (isBonusActive && currentCharge <= 0f)
        {
            DeactivateBonus();
        }
    }

    public void OnBasketScored()
    {
        if (isBonusActive) return;

        currentCharge += chargePerBasket;
        if (currentCharge >= bonusActivationThreshold && !isBonusActive)
        {
            ActivateBonus();
        }
    }
    public void OnBasketMissed()
    {
        currentCharge = 0f;
        if (isBonusActive)
        {
            DeactivateBonus();
        }
    }

    private void ActivateBonus()
    {
        isBonusActive = true;
    }
    private void DeactivateBonus()
    {
        isBonusActive = false;
    }

    public bool IsBonusActive()
    {
        return isBonusActive;
    }

    public int GetBonusMultiplier()
    {
        return bonusMultiplier;
    }
    public float GetCurrentCharge()
    {
        return currentCharge;
    }
    public float GetBonusActivationThreshold()
    {
        return bonusActivationThreshold;
    }
}
