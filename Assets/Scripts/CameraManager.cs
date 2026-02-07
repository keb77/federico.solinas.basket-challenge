using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private Transform ball;
    [SerializeField] private Transform hoop;

    [Header("Camera Settings")]
    [SerializeField] private float behindPlayerHeightOffset = 0.8f;
    [SerializeField] private float behindPlayerDepthOffset = 3f;
    [SerializeField] private float followingBallHeightOffset = 0.25f;
    [SerializeField] private float followingBallDepthOffset = 2f;
    [SerializeField] private float lockedOnHoopHeightOffset = 0f;
    [SerializeField] private float lockedOnHoopDepthOffset = 1.5f;
    [SerializeField] private float lockOnHoopDistance = 0.75f;
    [SerializeField] private float followSpeed = 5f;

    private enum CameraState
    {
        BehindPlayer,
        FollowingBall,
        LockedOnHoop
    }
    private CameraState currentState = CameraState.BehindPlayer;

    private Camera mainCamera;

    private void Awake()
    {
        Instance = this;

        mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        switch (currentState)
        {
            case CameraState.BehindPlayer:
                ResetCamera();
                break;
            case CameraState.FollowingBall:
                FollowBall();
                break;
            case CameraState.LockedOnHoop:
                LockOnHoop();
                break;
        }
    }

    private void FollowBall()
    {
        Vector3 targetRotation = hoop.position - ball.position;
        targetRotation.y = 0f;
        Vector3 targetPosition = ball.position - targetRotation.normalized * followingBallDepthOffset;
        targetPosition.y += followingBallHeightOffset;
        MoveCamera(targetPosition, targetRotation);

        float distanceToHoop = targetRotation.magnitude;
        if (distanceToHoop <= lockOnHoopDistance)
        {
            SetCameraLockedOnHoop();
        }
    }

    private void LockOnHoop()
    {
        Vector3 targetRotation = hoop.position - mainCamera.transform.position;
        targetRotation.y = 0f;
        Vector3 targetPosition = hoop.position - targetRotation.normalized * lockedOnHoopDepthOffset;
        targetPosition.y += lockedOnHoopHeightOffset;
        MoveCamera(targetPosition, targetRotation);
    }

    public void ResetCamera()
    {
        Vector3 targetRotation = hoop.position - ball.position;
        targetRotation.y = 0f;
        Vector3 targetPosition = ball.position - targetRotation.normalized * behindPlayerDepthOffset;
        targetPosition.y += behindPlayerHeightOffset;
        MoveCamera(targetPosition, targetRotation);
    }

    private void MoveCamera(Vector3 targetPosition, Vector3 targetRotation)
    {
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPosition, followSpeed * Time.deltaTime);
        mainCamera.transform.rotation = Quaternion.Slerp(mainCamera.transform.rotation, Quaternion.LookRotation(targetRotation), followSpeed * Time.deltaTime);
    }

    public void SetCameraBehindPlayer()
    {
        currentState = CameraState.BehindPlayer;
    }
    public void SetCameraFollowingBall()
    {
        currentState = CameraState.FollowingBall;
    }
    public void SetCameraLockedOnHoop()
    {
        currentState = CameraState.LockedOnHoop;
    }
}
