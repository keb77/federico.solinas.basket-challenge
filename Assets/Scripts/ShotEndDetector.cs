using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotEndDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            PlayerShooter.Instance.OnShotEnd(ScoreManager.Instance.HasScored);
        }
    }
}
