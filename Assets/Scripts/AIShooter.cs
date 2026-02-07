using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIShooter : Shooter
{
    [Header("Rookie Settings")]
    [SerializeField] private float rookieNoiseAmount = 0.1f;
    [SerializeField] private bool rookieCanGoForBackboard = false;
    [SerializeField] private float rookieDelayBeforeShoot = 2.0f;
    [Header("Pro Settings")]
    [SerializeField] private float proNoiseAmount = 0.07f;
    [SerializeField] private bool proCanGoForBackboard = true;
    [SerializeField] private float proDelayBeforeShoot = 1.5f;
    [Header("All Star Settings")]
    [SerializeField] private float allStarNoiseAmount = 0.04f;
    [SerializeField] private bool allStarCanGoForBackboard = true;
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

        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;

        GameMode gameMode = GameSettings.Instance != null ? GameSettings.Instance.CurrentGameMode : GameMode.Practice;
        if (gameMode == GameMode.Practice)
        {
            Destroy(transform.parent.gameObject);
            Destroy(ball);
            return;
        }
    }
    private void OnDisable()
    {
        GameManager.Instance.OnStateChanged -= GameManager_OnStateChanged;
    }
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
            yield return new WaitForSeconds(GetDelayBeforeShoot());

            TakeShot();

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

        float shotPower = ShotAccuracyManager.Instance.GetPerfectShotPower(transform, perfectTarget) + Random.Range(-noiseAmount, noiseAmount); ;
        if (canGoForBackboard && BackboardBonusManager.Instance.IsBonusActive)
        {
            shotPower += ShotAccuracyManager.Instance.GetBackboardShotOffset();
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
}