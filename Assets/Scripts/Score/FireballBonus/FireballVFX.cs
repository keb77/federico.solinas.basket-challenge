using UnityEngine;

/// Controls the visual effects for the fireball bonus.
public class FireballVFX : MonoBehaviour
{
    [SerializeField] private FireballHandler fireballHandler;
    [SerializeField] private GameObject fireballVFX;

    private void OnValidate()
    {
        if (fireballHandler == null || fireballVFX == null)
        {
            Debug.LogWarning("FireballVFX: Some fields are not assigned.", this);
        }
    }

    private void Start()
    {
        // Hide the VFX at the start
        fireballVFX.SetActive(false);

        fireballHandler.OnStateChanged += FireballHandler_OnStateChanged;
    }

    private void FireballHandler_OnStateChanged(object sender, System.EventArgs e)
    {
        if (fireballHandler.IsBonusActive())
        {
            fireballVFX.SetActive(true);
        }
        else
        {
            fireballVFX.SetActive(false);
        }
    }
    
    private void OnDestroy()
    {
        if (fireballHandler != null)
        {
            fireballHandler.OnStateChanged -= FireballHandler_OnStateChanged;
        }
    }
}
