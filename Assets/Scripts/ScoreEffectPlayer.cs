using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreEffectPlayer : MonoBehaviour
{
    [Header("VFX Settings")]
    [SerializeField] private ScoreFlyerGeneratorUI scoreFlyerGeneratorUI;

    [Header("SFX Settings")]
    [SerializeField] private AudioClip scoreSound;
    [SerializeField] private AudioSource audioSource;

    public void PlayScoreEffects(int scoreToAdd)
    {
        audioSource.PlayOneShot(scoreSound);
        
        scoreFlyerGeneratorUI.CreateScoreFlyer(scoreToAdd);
    }
}
