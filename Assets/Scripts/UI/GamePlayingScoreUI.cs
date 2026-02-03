using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GamePlayingScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Update()
    {
        if (GameManager.Instance.IsGamePlaying())
        {
            int score = ScoreManager.Instance.Score;
            scoreText.text = "Score: " + score.ToString();
        }
    }
}
