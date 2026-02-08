using UnityEngine;

/// Plays different music loops based on the game state (playing, victory, defeat, draw).
public class MusicLoopPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip gamePlayingLoop;
    [SerializeField] private AudioClip victoryLoop;
    [SerializeField] private AudioClip defeatLoop;
    [SerializeField] private AudioClip drawLoop;

    private void OnValidate()
    {
        if (audioSource == null || gamePlayingLoop == null || victoryLoop == null || defeatLoop == null || drawLoop == null)
        {
            Debug.LogWarning("MusicLoopPlayer: Some fields are not assigned.", this);
        }
    }

    private void Start()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager instance not found.");
            return;
        }
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

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnStateChanged -= GameManager_OnStateChanged;
        }
    }
}
