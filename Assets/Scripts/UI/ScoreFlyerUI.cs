using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreFlyerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private float floatUpSpeed = 1f;
    [SerializeField] private float lifeTime = 1f;

    private float timer = 0f;

    public void Initialize(int score)
    {
        scoreText.text = "+" + score.ToString();
    }

    private void Update()
    {
        Vector3 lookDirection = Camera.main.transform.position - transform.position;
        lookDirection.y = 0;
        transform.rotation = Quaternion.LookRotation(-lookDirection);
        
        transform.position += Vector3.up * floatUpSpeed * Time.deltaTime;

        timer += Time.deltaTime;
        float alpha = Mathf.Lerp(1f, 0f, timer / lifeTime);
        scoreText.color = new Color(scoreText.color.r, scoreText.color.g, scoreText.color.b, alpha);

        if (timer >= lifeTime)
        {
            Destroy(gameObject);
        }
    }
}
