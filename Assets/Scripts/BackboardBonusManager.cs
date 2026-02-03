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

    [SerializeField] private MeshRenderer backboardMeshRenderer;
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material commonBonusMaterial;
    [SerializeField] private Material uncommonBonusMaterial;
    [SerializeField] private Material rareBonusMaterial;

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

            Debug.Log($"Backboard Bonus Activated! +{CurrentBonusPoints} points for backboard shots.");
        }
    }

    public void ResetBonus()
    {
        IsBonusActive = false;
        CurrentBonusPoints = 0;
        SetDefaultMaterial();
    }

    public void SetMaterial(Material material)
    {
        Material[] materials = backboardMeshRenderer.materials;
        materials[0] = material;
        backboardMeshRenderer.materials = materials;
    }
    public void SetDefaultMaterial() => SetMaterial(defaultMaterial);
    public void SetCommonBonusMaterial() => SetMaterial(commonBonusMaterial);
    public void SetUncommonBonusMaterial() => SetMaterial(uncommonBonusMaterial);
    public void SetRareBonusMaterial() => SetMaterial(rareBonusMaterial);
}
