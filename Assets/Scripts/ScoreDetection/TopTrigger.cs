using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopTrigger : MonoBehaviour
{
    [SerializeField] private ScoreTrigger scoreTrigger;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Ball"))
        {
            scoreTrigger.OnTopTriggerEnter();
        }
    }
}
