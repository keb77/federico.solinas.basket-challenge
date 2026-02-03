using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreFlyerGeneratorUI : MonoBehaviour
{
    [SerializeField] private GameObject scoreFlyerUIPrefab;

    public void CreateScoreFlyer(int score)
    {
        GameObject flyer = Instantiate(scoreFlyerUIPrefab, transform.position, Quaternion.identity, transform);
        ScoreFlyerUI flyerUI = flyer.GetComponent<ScoreFlyerUI>();
        flyerUI.Initialize(score);
    }
}
