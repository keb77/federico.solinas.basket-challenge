using UnityEngine;
using System;

/// Handles player input for shooting mechanics. Supports both mouse and touch input.
public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private PlayerShooter playerShooter;

    [Header("Swipe Settings")]
    [Tooltip("Minimum swipe distance (percentage of screen height) to shoot.")]
    [SerializeField] private float minSwipeDistance = 0.05f;
    [Tooltip("Maximum swipe distance (percentage of screen height) for full shot power.")]
    [SerializeField] private float maxSwipeDistance = 0.6f;
    [Tooltip("Maximum swipe time (seconds) before a shot is automatically fired.")]
    [SerializeField] private float maxSwipeTime = 0.5f;

    private bool isTouching = false;
    private bool hasSwipeStarted = false;

    private float startPosition;
    private float swipeStartTime;
    private float currentSwipeMaxDistance;

    // Flag to enable or disable shooting input processing.
    public bool CanShoot { get; set; } = false;

    // Events to notify the swipe trail UI
    public event EventHandler OnSwipeStarted;
    public event EventHandler OnSwipeEnded;

    private void OnValidate()
    {
        if (playerShooter == null)
        {
            Debug.LogWarning("InputManager: Some fields are not assigned.", this);
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
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
            StartSwipe(currentPosition);
        }
        if (inputHold && isTouching)
        {
            UpdateSwipe(currentPosition);
        }
        if (inputUp && isTouching)
        {
            EndSwipe();
        }
    }

    /// Starts a swipe by recording the initial position and resetting relevant state variables.
    private void StartSwipe(float position)
    {
        isTouching = true;
        startPosition = position;
        currentSwipeMaxDistance = 0f;

        OnSwipeStarted?.Invoke(this, EventArgs.Empty);
    }
    
    /// Start a swipe if one is not yet started and it exceeds the minimum distance.
    /// If a swipe is already in progress, update the maximum swipe distance and check for auto-shooting based on time.
    private void UpdateSwipe(float position)
    {
        float swipeDistance = (position - startPosition) / Screen.height;

        if (!hasSwipeStarted)
        {
            if (swipeDistance >= minSwipeDistance)
            {
                hasSwipeStarted = true;
                swipeStartTime = Time.time;
                currentSwipeMaxDistance = swipeDistance;
            }
        }
        else
        {
            if (swipeDistance > currentSwipeMaxDistance)
            {
                currentSwipeMaxDistance = swipeDistance;
            }

            if (Time.time - swipeStartTime > maxSwipeTime)
            {
                playerShooter.Shoot(GetCurrentSwipeMaxDistanceNormalized());

                OnSwipeEnded?.Invoke(this, EventArgs.Empty);

                ResetInput();
            }
        }
    }

    /// Ends the swipe and triggers the shooting action if a swipe was in progress.
    private void EndSwipe()
    {
        if (hasSwipeStarted)
        {
            playerShooter.Shoot(GetCurrentSwipeMaxDistanceNormalized());

            OnSwipeEnded?.Invoke(this, EventArgs.Empty);
        }

        ResetInput();
    }

    private void ResetInput()
    {
        isTouching = false;
        hasSwipeStarted = false;
    }

    /// Returns the current swipe distance normalized to the maximum swipe distance, clamped between 0 and 1.
    /// This value represents the shot power percentage.
    public float GetCurrentSwipeMaxDistanceNormalized() => Mathf.Clamp01(currentSwipeMaxDistance / maxSwipeDistance);
    
    /// Resets the current swipe maximum distance to zero. Called when the shot ends to reset the input bar UI.
    public void ResetCurrentSwipeMaxDistance()
    {
        currentSwipeMaxDistance = 0f;
    }
}