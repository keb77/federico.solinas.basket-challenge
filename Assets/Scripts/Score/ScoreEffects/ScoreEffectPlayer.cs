using UnityEngine;

/// Plays effects (VFX and SFX) when the player scores.
public class ScoreEffectPlayer : MonoBehaviour
{
    [Header("VFX Settings")]
    [SerializeField] private ScoreFlyerGeneratorUI scoreFlyerGeneratorUI;
    [SerializeField] private GameObject scoreVFXPrefab;

    [Header("SFX Settings")]
    [SerializeField] private AudioClip scoreSound;
    [SerializeField] private AudioSource audioSource;

    private void OnValidate()
    {
        if (scoreFlyerGeneratorUI == null || scoreVFXPrefab == null || scoreSound == null || audioSource == null)
        {
            Debug.LogWarning("ScoreEffectPlayer: Some fields are not assigned.", this);
        }
    }

    public void PlayScoreEffects(int scoreToAdd)
    {
        // Play SFX
        audioSource.PlayOneShot(scoreSound);

        // Generate score flyer
        scoreFlyerGeneratorUI.CreateScoreFlyer(scoreToAdd);

        // Play VFX
        Instantiate(scoreVFXPrefab, transform.position, Quaternion.identity, transform);
    }
}
