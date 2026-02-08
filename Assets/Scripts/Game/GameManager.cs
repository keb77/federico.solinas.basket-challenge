using System;
using UnityEngine;

/// Manages the game state from start to finish.
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private PlayerShooter playerShooter;

    [Header("Timers")]
    [SerializeField] private float waitingToStartTimer = .5f;
    [SerializeField] private float countdownToStartTimer = 3f;
    [SerializeField] private float gamePlayingTimer = 120f;

    private float gamePlayingTimerMax;

    private enum GameState
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver,
    }
    private GameState currentState;
    public event EventHandler OnStateChanged;

    private void OnValidate()
    {
        if (playerShooter == null)
        {
            Debug.LogWarning("GameManager: Some fields are not assigned.", this);
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        currentState = GameState.WaitingToStart;

        gamePlayingTimerMax = gamePlayingTimer;
    }

    private void Update()
    {
        // Handle state transitions and timers
        switch (currentState)
        {
            case GameState.WaitingToStart:
                waitingToStartTimer -= Time.deltaTime;
                if (waitingToStartTimer <= 0f)
                {
                    SetState(GameState.CountdownToStart);
                }
                break;
            case GameState.CountdownToStart:
                countdownToStartTimer -= Time.deltaTime;
                if (countdownToStartTimer <= 0f)
                {
                    SetState(GameState.GamePlaying);
                    // Enable shooting when the game starts
                    InputManager.Instance.CanShoot = true;
                }
                break;
            case GameState.GamePlaying:
                gamePlayingTimer -= Time.deltaTime;
                // Wait until the shot ends before ending the game
                if (gamePlayingTimer <= 0f && !playerShooter.IsBallInPlay())
                {
                    SetState(GameState.GameOver);
                    // Disable shooting when the game ends
                    InputManager.Instance.CanShoot = false;
                }
                break;
            case GameState.GameOver:
                break;
        }
    }

    private void SetState(GameState newState)
    {
        currentState = newState;
        OnStateChanged?.Invoke(this, EventArgs.Empty);
    }


    public bool IsGamePlaying() => currentState == GameState.GamePlaying;
    public bool IsCountdownToStartActive() => currentState == GameState.CountdownToStart;
    public float GetCountdownToStartTimer() => countdownToStartTimer;
    public bool IsGameOver() => currentState == GameState.GameOver;

    // Returns a value between 0 and 1 representing how much time is left in the game. For UI purposes.
    public float GetGamePlayingTimerNormalized() => gamePlayingTimer / gamePlayingTimerMax;
}
