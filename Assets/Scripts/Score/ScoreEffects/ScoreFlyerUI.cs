using UnityEngine;
using TMPro;

/// Displays a floating score text that rises and fades out over time, facing the camera.
public class ScoreFlyerUI : MonoBehaviour
{
    [Tooltip("The TextMeshProUGUI component that displays the score.")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [Tooltip("The speed at which the score text floats upwards.")]
    [SerializeField] private float floatUpSpeed = 1f;
    [Tooltip("The total lifetime of the score text before it disappears.")]
    [SerializeField] private float lifeTime = 1f;

    private float timer = 0f;
    private Camera mainCamera;
    private Color initialTextColor;

    private void OnValidate()
    {
        if (scoreText == null)
        {
            Debug.LogWarning("ScoreFlyerUI: Some fields are not assigned.", this);
        }
    }

    private void Awake()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera not found in the scene.");
        }
    }

    public void Initialize(int score)
    {
        scoreText.text = "+" + score.ToString();

        initialTextColor = scoreText.color;
    }

    private void Update()
    {
        FaceCamera();
        MoveUpward();
        FadeOutAndDestroy();
    }

    private void FaceCamera()
    {
        Vector3 lookDirection = mainCamera.transform.position - transform.position;
        lookDirection.y = 0f;
        transform.rotation = Quaternion.LookRotation(-lookDirection);
    }

    private void MoveUpward()
    {
        transform.position += Vector3.up * floatUpSpeed * Time.deltaTime;
    }

    private void FadeOutAndDestroy()
    {
        timer += Time.deltaTime;

        float normalizedTime = Mathf.Clamp01(timer / lifeTime);
        float alpha = Mathf.Lerp(1f, 0f, normalizedTime);
        scoreText.color = new Color(
            initialTextColor.r,
            initialTextColor.g,
            initialTextColor.b,
            alpha
        );

        if (timer >= lifeTime)
        {
            Destroy(gameObject);
        }
    }
}
