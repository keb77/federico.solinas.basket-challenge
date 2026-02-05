using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class MusicLoopPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip gamePlayingLoop;
    [SerializeField] private AudioClip victoryLoop;
    [SerializeField] private AudioClip defeatLoop;
    [SerializeField] private AudioClip drawLoop;

    private void Start()
    {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
    }

    private void GameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsGamePlaying())
        {
            PlayLoop(gamePlayingLoop);
        }
        else if (GameManager.Instance.IsGameOver())
        {
            if (GameSettings.Instance == null || GameSettings.Instance.CurrentGameMode == GameMode.Practice)
            {
                PlayLoop(victoryLoop);
                return;
            }
            else
            {
                int playerScore = ScoreManager.Instance.GetScore(ShooterType.Player);
                int aiScore = ScoreManager.Instance.GetScore(ShooterType.AI);
                if (playerScore > aiScore)
                {
                    PlayLoop(victoryLoop);
                }
                else if (playerScore < aiScore)
                {
                    PlayLoop(defeatLoop);
                }
                else
                {
                    PlayLoop(drawLoop);
                }
            }
        }
    }

    private void PlayLoop(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
}
