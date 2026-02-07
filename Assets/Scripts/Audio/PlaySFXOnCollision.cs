using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySFXOnCollision : MonoBehaviour
{
    [SerializeField] private AudioClip collisionSound;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private string targetTag = "PlayerBall";
    [SerializeField] private float maxVolume = 0.3f;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(targetTag))
        {
            float volume = Mathf.Clamp(collision.relativeVelocity.magnitude / 10f, 0f, maxVolume);
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(collisionSound, volume);
        }
    }
}
