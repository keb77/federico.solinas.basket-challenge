using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    [SerializeField] private PlayerShooter playerShooter;

    [SerializeField] private float minSwipeDistance = 0.05f; // screen height percentage
    [SerializeField] private float maxSwipeDistance = 0.6f;
    [SerializeField] private float maxSwipeTime = 0.5f; // seconds

    private bool isTouching = false;
    private bool hasSwipeStarted = false;

    private float startPosition;
    private float swipeStartTime;
    private float currentSwipeMaxDistance;

    public bool CanShoot { get; set; } = false;

    public event EventHandler OnSwipeStarted;
    public event EventHandler OnSwipeEnded;

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

                    OnSwipeStarted?.Invoke(this, EventArgs.Empty);
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
                    playerShooter.Shoot(GetCurrentSwipeMaxDistanceNormalized());

                    OnSwipeEnded?.Invoke(this, EventArgs.Empty);

                    ResetInput();
                }
            }
        }
        if (inputUp && isTouching)
        {
            if (hasSwipeStarted)
            {
                playerShooter.Shoot(GetCurrentSwipeMaxDistanceNormalized());

                OnSwipeEnded?.Invoke(this, EventArgs.Empty);
            }

            ResetInput();
        }
    }

    private void ResetInput()
    {
        isTouching = false;
        hasSwipeStarted = false;
    }

    public float GetCurrentSwipeMaxDistanceNormalized()
    {
        return Mathf.Clamp01(currentSwipeMaxDistance / maxSwipeDistance);
    }
    public void ResetCurrentSwipeMaxDistance()
    {
        currentSwipeMaxDistance = 0f;
    }

    public Vector2 GetInputPosition()
    {
        if (Input.touchCount > 0)
        {
            return Input.GetTouch(0).position;
        }
        else if (Input.GetMouseButton(0))
        {
            return Input.mousePosition;
        }
        return Vector2.zero;
    }
}
