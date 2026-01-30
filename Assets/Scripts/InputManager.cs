using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public float minSwipeDistance = 50f;
    public float maxSwipeTime = 0.5f;

    private bool isTouching = false;
    private bool hasSwipeStarted = false;
    private float startPosition;
    private float swipeStartTime;

    private void Update()
    {
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
        }
        if (inputHold && isTouching)
        {
            if (!hasSwipeStarted)
            {
                if (currentPosition - startPosition > minSwipeDistance)
                {
                    hasSwipeStarted = true;
                    swipeStartTime = Time.time;
                    Debug.Log("Swipe Started");
                }
            }
            else
            {
                // Update UI and feedback here

                if (Time.time - swipeStartTime > maxSwipeTime)
                {
                    // Shoot here
                    Debug.Log("Swipe Timeout. Distance: " + (currentPosition - startPosition));

                    ResetInput();
                }
            }
        }
        if (inputUp && isTouching)
        {
            if (hasSwipeStarted)
            {
                // Shoot here
                Debug.Log("Swipe Ended. Distance: " + (currentPosition - startPosition));
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
