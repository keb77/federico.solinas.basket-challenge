using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ScoreEffectPlayer : MonoBehaviour
{
    [Header("VFX Settings")]
    [SerializeField] private ScoreFlyerGeneratorUI scoreFlyerGeneratorUI;
    [SerializeField] private GameObject scoreVFXPrefab;

    [Header("SFX Settings")]
    [SerializeField] private AudioClip scoreSound;
    [SerializeField] private AudioSource audioSource;

    public void PlayScoreEffects(int scoreToAdd)
    {
        audioSource.PlayOneShot(scoreSound);

        scoreFlyerGeneratorUI.CreateScoreFlyer(scoreToAdd);

        Instantiate(scoreVFXPrefab, transform.position, Quaternion.identity, transform);
    }
}
