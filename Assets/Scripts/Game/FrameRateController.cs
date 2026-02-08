using UnityEngine;

public class FrameRateController : MonoBehaviour
{
    [SerializeField] private int targetFrameRate = 120;

    private void Awake()
    {
        Application.targetFrameRate = targetFrameRate;
    }
}
