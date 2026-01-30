using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreTrigger : MonoBehaviour
{
    private bool ballEnteredTop = false;
    private bool alreadyScored = false;

    public void OnTopTriggerEnter()
    {
        if (alreadyScored) return;

        ballEnteredTop = true;
    }

    public void OnBottomTriggerExit()
    {
        if (alreadyScored) return;
        
        if (ballEnteredTop)
        {
            Debug.Log("Score!");
            alreadyScored = true;
        }
    }

    public void ResetScoreTrigger()
    {
        ballEnteredTop = false;
        alreadyScored = false;
    }
}
