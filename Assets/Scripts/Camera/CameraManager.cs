using UnityEngine;

/// Manages the camera behavior in the game. 
/// The camera stays behind the player by default, follows the ball when shot, and locks on to the hoop when close enough.
public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private Transform ball;
    [SerializeField] private Transform hoopPerfectTarget;

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

    private void OnValidate()
    {
        if (ball == null || hoopPerfectTarget == null)
        {
            Debug.LogWarning("CameraManager: Some fields are not assigned.", this);
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

        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera not found in the scene.");
            enabled = false;
        }
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

    /// Follows the ball until close enough to lock on to the hoop.
    private void FollowBall()
    {
        Vector3 targetRotation = hoopPerfectTarget.position - ball.position;
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

    /// Locks the camera on the hoop.
    private void LockOnHoop()
    {
        Vector3 targetRotation = hoopPerfectTarget.position - mainCamera.transform.position;
        targetRotation.y = 0f;
        Vector3 targetPosition = hoopPerfectTarget.position - targetRotation.normalized * lockedOnHoopDepthOffset;
        targetPosition.y += lockedOnHoopHeightOffset;
        MoveCamera(targetPosition, targetRotation);
    }

    /// Resets the camera to behind the player.
    private void ResetCamera()
    {
        Vector3 targetRotation = hoopPerfectTarget.position - ball.position;
        targetRotation.y = 0f;
        Vector3 targetPosition = ball.position - targetRotation.normalized * behindPlayerDepthOffset;
        targetPosition.y += behindPlayerHeightOffset;
        MoveCamera(targetPosition, targetRotation);
    }

    /// Moves the camera towards the target position and rotation smoothly.
    private void MoveCamera(Vector3 targetPosition, Vector3 targetRotation)
    {
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPosition, followSpeed * Time.deltaTime);
        mainCamera.transform.rotation = Quaternion.Slerp(mainCamera.transform.rotation, Quaternion.LookRotation(targetRotation), followSpeed * Time.deltaTime);
    }

    public void SetCameraBehindPlayer() => currentState = CameraState.BehindPlayer;
    public void SetCameraFollowingBall() => currentState = CameraState.FollowingBall;
    public void SetCameraLockedOnHoop() => currentState = CameraState.LockedOnHoop;
}
