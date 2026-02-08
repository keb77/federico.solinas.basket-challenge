using System.Collections;
using UnityEngine;

/// Handles AI shooting mechanics, including shot power generation based on difficulty settings, timing of shots, and interaction with game state changes.
public class AIShooter : Shooter
{
    [Header("Rookie Settings")]
    [Tooltip("Amount of random noise added to the shot power for Rookie difficulty. Higher values result in less accurate shots.")]
    [SerializeField] private float rookieNoiseAmount = 0.1f;
    [Tooltip("Determines whether the AI can attempt backboard shots on Rookie difficulty.")]
    [SerializeField] private bool rookieCanGoForBackboard = false;
    [Tooltip("Delay in seconds before the AI takes a shot on Rookie difficulty. Higher values result in slower shooting.")]
    [SerializeField] private float rookieDelayBeforeShoot = 2.0f;
    [Header("Pro Settings")]
    [Tooltip("Amount of random noise added to the shot power for Pro difficulty. Higher values result in less accurate shots.")]
    [SerializeField] private float proNoiseAmount = 0.07f;
    [Tooltip("Determines whether the AI can attempt backboard shots on Pro difficulty.")]
    [SerializeField] private bool proCanGoForBackboard = true;
    [Tooltip("Delay in seconds before the AI takes a shot on Pro difficulty. Higher values result in slower shooting.")]
    [SerializeField] private float proDelayBeforeShoot = 1.5f;
    [Header("All Star Settings")]
    [Tooltip("Amount of random noise added to the shot power for All Star difficulty. Higher values result in less accurate shots.")]
    [SerializeField] private float allStarNoiseAmount = 0.04f;
    [Tooltip("Determines whether the AI can attempt backboard shots on All Star difficulty.")]
    [SerializeField] private bool allStarCanGoForBackboard = true;
    [Tooltip("Delay in seconds before the AI takes a shot on All Star difficulty. Higher values result in slower shooting.")]
    [SerializeField] private float allStarDelayBeforeShoot = 1.0f;

    private AIDifficulty difficultyLevel = AIDifficulty.Rookie;
    private Coroutine shootingCoroutine;

    protected override void Awake()
    {
        ShooterType = ShooterType.AI;
        base.Awake();
    }

    private void Start()
    {
        if (GameSettings.Instance != null)
        {
            difficultyLevel = GameSettings.Instance.CurrentAIDifficulty;
        }

        // Destroy AI shooter if in practice mode
        GameMode gameMode = GameSettings.Instance != null ? GameSettings.Instance.CurrentGameMode : GameMode.Practice;
        if (gameMode == GameMode.Practice)
        {
            Destroy(transform.parent.gameObject);
            return;
        }

        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager instance is missing.");
            return;
        }
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
    }

    /// Start or stop the shooting routine based on the current game state. 
    private void GameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsGamePlaying())
        {
            StartShootingRoutine();
        }
        else
        {
            StopShootingRoutine();
        }
    }

    private void StartShootingRoutine()
    {
        if (shootingCoroutine != null)
        {
            StopCoroutine(shootingCoroutine);
        }
        shootingCoroutine = StartCoroutine(ShootingRoutine());
    }

    private void StopShootingRoutine()
    {
        if (shootingCoroutine != null)
        {
            StopCoroutine(shootingCoroutine);
            shootingCoroutine = null;
        }
    }

    private IEnumerator ShootingRoutine()
    {
        while (GameManager.Instance.IsGamePlaying())
        {
            // Wait for the specified delay before taking a shot
            yield return new WaitForSeconds(GetDelayBeforeShoot());

            TakeShot();

            // Wait until the shot has ended before taking the next shot
            yield return new WaitUntil(() => !IsBallInPlay());
        }
    }

    private void TakeShot()
    {
        float shotPower = GenerateShotPower();
        Shoot(shotPower);
    }

    private float GenerateShotPower()
    {
        float noiseAmount = 0f;
        bool canGoForBackboard = false;

        // Determine noise amount and backboard shot capability
        switch (difficultyLevel)
        {
            case AIDifficulty.Rookie:
                noiseAmount = rookieNoiseAmount;
                canGoForBackboard = rookieCanGoForBackboard;
                break;
            case AIDifficulty.Pro:
                noiseAmount = proNoiseAmount;
                canGoForBackboard = proCanGoForBackboard;
                break;
            case AIDifficulty.AllStar:
                noiseAmount = allStarNoiseAmount;
                canGoForBackboard = allStarCanGoForBackboard;
                break;
        }

        // Get the perfect shot power and add random noise to it
        float shotPower = shotAccuracyManager.GetPerfectShotPower(transform, perfectTarget) + Random.Range(-noiseAmount, noiseAmount);

        // Add the backboard shot offset if needed
        if (canGoForBackboard && backboardBonusManager.IsBonusActive)
        {
            shotPower += shotAccuracyManager.GetBackboardShotOffset();
        }

        return shotPower;
    }

    private float GetDelayBeforeShoot()
    {
        switch (difficultyLevel)
        {
            case AIDifficulty.Rookie:
                return rookieDelayBeforeShoot;
            case AIDifficulty.Pro:
                return proDelayBeforeShoot;
            case AIDifficulty.AllStar:
                return allStarDelayBeforeShoot;
            default:
                return rookieDelayBeforeShoot;
        }
    }
    
    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnStateChanged -= GameManager_OnStateChanged;
        }
    }
}