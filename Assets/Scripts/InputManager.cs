using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public float minSwipeDistance = 0.05f; // screen height percentage
    public float maxSwipeDistance = 0.6f;
    public float maxSwipeTime = 0.5f; // seconds

    private bool isTouching = false;
    private bool hasSwipeStarted = false;

    private float startPosition;
    private float swipeStartTime;
    private float currentSwipeMaxDistance;

    public bool CanShoot { get; set; } = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (!CanShoot) return;

        bool inputDown = false;
        bool inputHold = false;
        bool inputUp = false;
        float currentPosition = 0f;

        // Mouse Input
        if (Input.GetMouseButtonDown(0))
        {
            inputDown = true;
            currentPosition = Input.mousePosition.y;
        }
        else if (Input.GetMouseButton(0))
        {
            inputHold = true;
            currentPosition = Input.mousePosition.y;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            inputUp = true;
            currentPosition = Input.mousePosition.y;
        }

        // Touch Input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            currentPosition = touch.position.y;

            if (touch.phase == TouchPhase.Began)
            {
                inputDown = true;
            }
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                inputHold = true;
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                inputUp = true;
            }
        }

        // Process Input
        if (inputDown)
        {
            isTouching = true;
            startPosition = currentPosition;
            currentSwipeMaxDistance = 0f;
        }
        if (inputHold && isTouching)
        {
            float currentSwipeDistance = (currentPosition - startPosition) / Screen.height;

            if (!hasSwipeStarted)
            {
                if (currentSwipeDistance >= minSwipeDistance)
                {
                    hasSwipeStarted = true;
                    swipeStartTime = Time.time;
                    currentSwipeMaxDistance = currentSwipeDistance;
                }
            }
            else
            {
                if (currentSwipeDistance > currentSwipeMaxDistance)
                {
                    currentSwipeMaxDistance = currentSwipeDistance;
                }

                if (Time.time - swipeStartTime > maxSwipeTime)
                {
                    PlayerShooter.Instance.Shoot(Mathf.Clamp01(currentSwipeMaxDistance / maxSwipeDistance));

                    ResetInput();
                }
            }
        }
        if (inputUp && isTouching)
        {
            if (hasSwipeStarted)
            {
                PlayerShooter.Instance.Shoot(Mathf.Clamp01(currentSwipeMaxDistance / maxSwipeDistance));
            }

            ResetInput();
        }
    }

    private void ResetInput()
    {
        isTouching = false;
        hasSwipeStarted = false;
    }
}
