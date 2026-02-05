using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerShooter : Shooter
{
    [Header("SFX Settings")]
    [SerializeField] private AudioSource shootSFXSource;
    [SerializeField] private AudioClip shootSFX;

    protected override void Awake()
    {
        ShooterType = ShooterType.Player;
        base.Awake();
    }

    public override void Shoot(float shotPower)
    {
        base.Shoot(shotPower);

        InputManager.Instance.CanShoot = false;
        CameraManager.Instance.SetCameraFollowingBall();

        shootSFXSource.PlayOneShot(shootSFX);
    }

    public override void OnShotEnd(bool hasScored)
    {
        base.OnShotEnd(hasScored);
        
        if (hasScored)
        {
            BackboardBonusManager.Instance.TryActivateBonus();
        }
        
        CameraManager.Instance.SetCameraBehindPlayer();
        if (GameManager.Instance.IsGamePlaying())
        {
            InputManager.Instance.CanShoot = true;
            InputManager.Instance.ResetCurrentSwipeMaxDistance();
        }
    }
}
