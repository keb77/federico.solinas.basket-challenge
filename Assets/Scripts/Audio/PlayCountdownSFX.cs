using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayCountdownSFX : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    private void Start()
    {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
    }

    private void GameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsCountdownToStartActive())
        {
            audioSource.Play();
        }
    }
}
