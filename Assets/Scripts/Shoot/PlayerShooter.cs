using UnityEngine;

/// Handles player shooting mechanics, including input management, camera control, and sound effects.
public class PlayerShooter : Shooter
{
    [Header("SFX Settings")]
    [SerializeField] private AudioSource shootSFXSource;
    [SerializeField] private AudioClip shootSFX;

    private void OnValidate()
    {
        if (shootSFXSource == null || shootSFX == null)
        {
            Debug.LogWarning("PlayerShooter: Some fields are not assigned.", this);
        }
    }

    protected override void Awake()
    {
        ShooterType = ShooterType.Player;
        base.Awake();
    }

    public override void Shoot(float shotPower)
    {
        base.Shoot(shotPower);

        // Play shooting sound effect
        shootSFXSource.PlayOneShot(shootSFX);

        // Disable player input and set camera to follow the ball during the shot
        if (InputManager.Instance == null || CameraManager.Instance == null)
        {
            Debug.LogWarning("InputManager or CameraManager instance is missing.");
            return;
        }
        InputManager.Instance.CanShoot = false;
        CameraManager.Instance.SetCameraFollowingBall();
    }

    public override void OnShotEnd(bool hasScored)
    {
        base.OnShotEnd(hasScored);

        // Try to activate backboard bonus if the player scored
        if (hasScored)
        {
            backboardBonusManager.TryActivateBonus();
        }

        // Reset camera behind the player and re-enable input if the game is still playing
        if (InputManager.Instance == null || CameraManager.Instance == null || GameManager.Instance == null)
        {
            Debug.LogWarning("InputManager, CameraManager, or GameManager instance is missing.");
            return;
        }
        CameraManager.Instance.SetCameraBehindPlayer();
        if (GameManager.Instance.IsGamePlaying())
        {
            InputManager.Instance.CanShoot = true;
            InputManager.Instance.ResetCurrentSwipeMaxDistance();
        }
    }
}
