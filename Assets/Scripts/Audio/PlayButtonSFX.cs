using UnityEngine;

/// Plays a sound effect when a button is pressed.
public class PlayButtonSFX : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clickSound;
    
    private void OnValidate()
    {
        if (audioSource == null || clickSound == null)
        {
            Debug.LogWarning("PlayButtonSFX: Some fields are not assigned.", this);
        }
    }

    private void Awake()
    {
        // Ensure the sound effect persists across scene loads
        DontDestroyOnLoad(gameObject);
    }

    /// Assign this method to the button's OnClick event.
    public void PlayClick()
    {
        audioSource.PlayOneShot(clickSound);
    }
}
