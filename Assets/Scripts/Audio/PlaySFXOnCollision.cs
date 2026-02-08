using UnityEngine;

// Plays a sound effect when the object collides with another object that has a specific tag (e.g., the player's ball).
// Attach this to surfaces that the ball will collide with to add audio feedback for collisions.
public class PlaySFXOnCollision : MonoBehaviour
{
    [SerializeField] private AudioClip collisionSound;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private string targetTag = "PlayerBall";
    [SerializeField] private float maxVolume = 1f;

    private void OnValidate()
    {
        if (audioSource == null || collisionSound == null)
        {
            Debug.LogWarning("PlaySFXOnCollision: Some fields are not assigned.", this);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(targetTag))
        {
            // Scale volume based on collision intensity
            float volume = Mathf.Clamp(collision.relativeVelocity.magnitude / 10f, 0f, maxVolume);
            // Randomize pitch for variety
            audioSource.pitch = Random.Range(0.9f, 1.1f);

            audioSource.PlayOneShot(collisionSound, volume);
        }
    }
}
