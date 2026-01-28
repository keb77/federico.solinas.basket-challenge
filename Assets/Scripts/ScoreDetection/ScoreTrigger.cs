using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreTrigger : MonoBehaviour
{
    private bool ballEnteredTop = false;

    public void OnTopTriggerEnter()
    {
        ballEnteredTop = true;
    }

    public void OnBottomTriggerExit()
    {
        if (ballEnteredTop)
        {
            Debug.Log("Score!");
            ResetScoreTrigger();
        }
    }
    
    public void ResetScoreTrigger()
    {
        ballEnteredTop = false;
    }
}
