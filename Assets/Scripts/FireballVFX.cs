using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FireballVFX : MonoBehaviour
{
    [SerializeField] private FireballHandler fireballHandler;
    [SerializeField] private GameObject fireballVFX;

    private void Start()
    {
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
}
