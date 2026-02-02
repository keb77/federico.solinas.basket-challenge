using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackboardBonusManager : MonoBehaviour
{
    public static BackboardBonusManager Instance { get; private set; }

    [SerializeField] private float activationChance = 0.2f;
    [SerializeField] private float commonBonusChance = 0.5f;
    [SerializeField] private float uncommonBonusChance = 0.35f;
    [SerializeField] private int commonBonusPoints = 4;
    [SerializeField] private int uncommonBonusPoints = 6;
    [SerializeField] private int rareBonusPoints = 8;

    public bool IsBonusActive { get; private set; } = false;
    public int CurrentBonusPoints {get; private set; } = 0;

    private void Awake()
    {
        Instance = this;
    }

    public void TryActivateBonus()
    {
        if (IsBonusActive) return;

        if (Random.value < activationChance)
        {
            IsBonusActive = true;

            float bonusRoll = Random.value;
            if (bonusRoll < commonBonusChance)
            {
                CurrentBonusPoints = commonBonusPoints;
            }
            else if (bonusRoll < commonBonusChance + uncommonBonusChance)
            {
                CurrentBonusPoints = uncommonBonusPoints;
            }
            else
            {
                CurrentBonusPoints = rareBonusPoints;
            }

            Debug.Log($"Backboard Bonus Activated. Bonus Points: {CurrentBonusPoints}");
        }
    }

    public void ResetBonus()
    {
        IsBonusActive = false;
        CurrentBonusPoints = 0;
    }
}
