using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private enum GameState
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver,
    }
    private GameState currentState;

    [SerializeField] private float waitingToStartTimer = .5f;
    [SerializeField] private float countdownToStartTimer = 3f;
    [SerializeField] private float gamePlayingTimer = 120f;
    private float gamePlayingTimerMax;

    public event EventHandler OnStateChanged;

    private void Awake()
    {
        Instance = this;

        currentState = GameState.WaitingToStart;

        gamePlayingTimerMax = gamePlayingTimer;
    }

    private void Update()
    {
        switch (currentState)
        {
            case GameState.WaitingToStart:
                waitingToStartTimer -= Time.deltaTime;
                if (waitingToStartTimer <= 0f)
                {
                    currentState = GameState.CountdownToStart;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case GameState.CountdownToStart:
                countdownToStartTimer -= Time.deltaTime;
                if (countdownToStartTimer <= 0f)
                {
                    currentState = GameState.GamePlaying;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                    InputManager.Instance.CanShoot = true;
                }
                break;
            case GameState.GamePlaying:
                gamePlayingTimer -= Time.deltaTime;
                if (gamePlayingTimer <= 0f && !PlayerShooter.Instance.IsBallInPlay())
                {
                    currentState = GameState.GameOver;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                    InputManager.Instance.CanShoot = false;
                }
                break;
            case GameState.GameOver:
                break;
        }
    }

    public bool IsGamePlaying()
    {
        return currentState == GameState.GamePlaying;
    }

    public bool IsCountdownToStartActive()
    {
        return currentState == GameState.CountdownToStart;
    }
    public float GetCountdownToStartTimer()
    {
        return countdownToStartTimer;
    }

    public bool IsGameOver()
    {
        return currentState == GameState.GameOver;
    }

    public float GetGamePlayingTimerNormalized()
    {
        return gamePlayingTimer / gamePlayingTimerMax;
    }
}
