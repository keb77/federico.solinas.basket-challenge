using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomTrigger : MonoBehaviour
{
    [SerializeField] private ScoreTrigger scoreTrigger;

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Ball"))
        {
            scoreTrigger.OnBottomTriggerExit();
        }
    }
}
