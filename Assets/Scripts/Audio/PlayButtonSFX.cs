using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButtonSFX : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip clickSound;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    
    public void PlayClick()
    {
        audioSource.PlayOneShot(clickSound);
    }
}
