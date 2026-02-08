using TMPro;
using UnityEngine;

/// Manages the backboard bonus system, including activation, bonus type determination, and visual feedback.
public class BackboardBonusManager : MonoBehaviour
{
    [Header("References")]
    [Tooltip("MeshRenderer for the backboard to change materials based on bonus type.")]
    [SerializeField] private MeshRenderer backboardMeshRenderer;
    [Tooltip("Default material for the backboard when no bonus is active.")]
    [SerializeField] private Material defaultMaterial;
    [Tooltip("Material for the backboard when a common bonus is active.")]
    [SerializeField] private Material commonBonusMaterial;
    [Tooltip("Material for the backboard when an uncommon bonus is active.")]
    [SerializeField] private Material uncommonBonusMaterial;
    [Tooltip("Material for the backboard when a rare bonus is active.")]
    [SerializeField] private Material rareBonusMaterial;
    [Tooltip("Text element to display the current bonus points when a bonus is active.")]
    [SerializeField] private TextMeshProUGUI bonusText;

    [Header("Bonus Settings")]
    [Tooltip("Chance for the backboard bonus to activate.")]
    [SerializeField] private float activationChance = 0.2f;
    [Tooltip("Chance for the bonus to be common when activated.")]
    [SerializeField] private float commonBonusChance = 0.5f;
    [Tooltip("Chance for the bonus to be uncommon when activated.")]
    [SerializeField] private float uncommonBonusChance = 0.35f;
    // The remaining chance (1 - commonBonusChance - uncommonBonusChance) will be for the rare bonus.

    [Tooltip("Additional points awarded for a common bonus.")]
    [SerializeField] private int commonBonusPoints = 4;
    [Tooltip("Additional points awarded for an uncommon bonus.")]
    [SerializeField] private int uncommonBonusPoints = 6;
    [Tooltip("Additional points awarded for a rare bonus.")]
    [SerializeField] private int rareBonusPoints = 8;

    public bool IsBonusActive { get; private set; } = false;
    public int CurrentBonusPoints { get; private set; } = 0;

    private void OnValidate()
    {
        if (backboardMeshRenderer == null || defaultMaterial == null || commonBonusMaterial == null ||
            uncommonBonusMaterial == null || rareBonusMaterial == null || bonusText == null)
        {
            Debug.LogWarning("BackboardBonusManager: Some fields are not assigned.", this);
        }

        if (activationChance < 0f || activationChance > 1f)
        {
            Debug.LogWarning("BackboardBonusManager: Activation chance should be between 0 and 1.", this);
        }
        if(commonBonusChance < 0f || uncommonBonusChance < 0f || uncommonBonusChance > 1f || commonBonusChance + uncommonBonusChance > 1f)
        {
            Debug.LogWarning("BackboardBonusManager: Bonus type chances should be between 0 and 1, and their sum should not exceed 1.", this);
        }
    }

    /// Attempts to activate the backboard bonus based on probability. Update visuals accordingly if activated.
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

            SetBonusText();
        }
    }

    public void ResetBonus()
    {
        IsBonusActive = false;
        CurrentBonusPoints = 0;
        SetDefaultMaterial();
        SetBonusText();
    }

    private void SetMaterial(Material material)
    {   
        Material[] materials = backboardMeshRenderer.materials;
        if (materials.Length == 0) return;

        materials[0] = material;
        backboardMeshRenderer.materials = materials;
    }
    private void SetDefaultMaterial() => SetMaterial(defaultMaterial);
    private void SetCommonBonusMaterial() => SetMaterial(commonBonusMaterial);
    private void SetUncommonBonusMaterial() => SetMaterial(uncommonBonusMaterial);
    private void SetRareBonusMaterial() => SetMaterial(rareBonusMaterial);

    private void SetBonusText()
    {
        if (IsBonusActive)
        {
            bonusText.text = "+" + CurrentBonusPoints.ToString();
        }
        else
        {
            bonusText.text = "";
        }
    }
}
