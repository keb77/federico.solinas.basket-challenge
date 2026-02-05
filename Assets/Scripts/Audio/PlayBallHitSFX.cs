using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBallHitSFX : MonoBehaviour
{
    [SerializeField] private AudioClip collisionSound;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float maxVolume = 0.3f;
    [SerializeField] private string targetTag = "Hoop";

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(targetTag))
        {
            float volume = Mathf.Clamp(collision.relativeVelocity.magnitude / 10f, 0f, maxVolume);
            audioSource.pitch = Random.Range(0.5f, 1.5f);
            audioSource.PlayOneShot(collisionSound, volume);
        }
    }
}
