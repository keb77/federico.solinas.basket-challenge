using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BackboardBonusManager : MonoBehaviour
{
    public static BackboardBonusManager Instance { get; private set; }

    [Header("Bonus Settings")]
    [SerializeField] private float activationChance = 0.2f;
    [SerializeField] private float commonBonusChance = 0.5f;
    [SerializeField] private float uncommonBonusChance = 0.35f;
    [SerializeField] private int commonBonusPoints = 4;
    [SerializeField] private int uncommonBonusPoints = 6;
    [SerializeField] private int rareBonusPoints = 8;

    [Header("References")]
    [SerializeField] private MeshRenderer backboardMeshRenderer;
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material commonBonusMaterial;
    [SerializeField] private Material uncommonBonusMaterial;
    [SerializeField] private Material rareBonusMaterial;
    [SerializeField] private TextMeshProUGUI bonusText;

    public bool IsBonusActive { get; private set; } = false;
    public int CurrentBonusPoints { get; private set; } = 0;

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
                SetCommonBonusMaterial();
            }
            else if (bonusRoll < commonBonusChance + uncommonBonusChance)
            {
                CurrentBonusPoints = uncommonBonusPoints;
                SetUncommonBonusMaterial();
            }
            else
            {
                CurrentBonusPoints = rareBonusPoints;
                SetRareBonusMaterial();
            }

            SetBonusText(CurrentBonusPoints);
        }
    }

    public void ResetBonus()
    {
        IsBonusActive = false;
        CurrentBonusPoints = 0;
        SetDefaultMaterial();
        SetBonusText(0);
    }

    private void SetMaterial(Material material)
    {
        Material[] materials = backboardMeshRenderer.materials;
        materials[0] = material;
        backboardMeshRenderer.materials = materials;
    }
    private void SetDefaultMaterial() => SetMaterial(defaultMaterial);
    private void SetCommonBonusMaterial() => SetMaterial(commonBonusMaterial);
    private void SetUncommonBonusMaterial() => SetMaterial(uncommonBonusMaterial);
    private void SetRareBonusMaterial() => SetMaterial(rareBonusMaterial);

    private void SetBonusText(int bonusPoints) => bonusText.text = bonusPoints > 0 ? "+" + bonusPoints.ToString() : "";
}
