using UnityEngine;

/// Generates score flyer UI elements.
public class ScoreFlyerGeneratorUI : MonoBehaviour
{
    [SerializeField] private GameObject scoreFlyerUIPrefab;
    
    private void OnValidate()
    {
        if (scoreFlyerUIPrefab == null)
        {
            Debug.LogWarning("ScoreFlyerGeneratorUI: Some fields are not assigned.", this);
        }
    }

    public void CreateScoreFlyer(int score)
    {
        GameObject flyer = Instantiate(scoreFlyerUIPrefab, transform.position, Quaternion.identity, transform);
        ScoreFlyerUI flyerUI = flyer.GetComponent<ScoreFlyerUI>();
        flyerUI.Initialize(score);
    }
}
