using UnityEngine;
using System;

/// Handles the logic for the Fireball Bonus system.
public class FireballHandler : MonoBehaviour
{
    [Tooltip("How much charge is gained per successful basket.")]
    [SerializeField] private float chargePerBasket = 2f;
    [Tooltip("The charge threshold required to activate the bonus.")]
    [SerializeField] private float bonusActivationThreshold = 5f;
    [Tooltip("The rate at which charge decays when the bonus is inactive.")]
    [SerializeField] private float decaySpeedWhenInactive = 0.2f;
    [Tooltip("The rate at which charge decays when the bonus is active.")]
    [SerializeField] private float decaySpeedWhenActive = 0.4f;
    [Tooltip("The score multiplier applied when the bonus is active.")]
    [SerializeField] private int bonusMultiplier = 2;

    // Notify the VFX player when the bonus state changes
    public event EventHandler OnStateChanged;

    private float currentCharge = 0f;
    private bool isBonusActive = false;

    private void Update()
    {
        // Decay the charge over time and deactivate the bonus if the charge runs out while active
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

        // Increase charge and activate bonus if threshold is reached
        currentCharge += chargePerBasket;
        if (currentCharge >= bonusActivationThreshold && !isBonusActive)
        {
            ActivateBonus();
        }
    }
    public void OnBasketMissed()
    {
        // Reset charge and deactivate bonus if missed while active
        currentCharge = 0f;
        if (isBonusActive)
        {
            DeactivateBonus();
        }
    }

    private void ActivateBonus()
    {
        isBonusActive = true;

        OnStateChanged?.Invoke(this, EventArgs.Empty);
    }
    private void DeactivateBonus()
    {
        isBonusActive = false;

        OnStateChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool IsBonusActive() => isBonusActive;
    public int GetBonusMultiplier() => bonusMultiplier;
    public float GetCurrentCharge() => currentCharge;
    public float GetBonusActivationThreshold() => bonusActivationThreshold;
}
