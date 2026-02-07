using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwipeTrailUI : MonoBehaviour
{
    [SerializeField] private LineRenderer swipeTrailUIPrefab;
    [SerializeField] private float clearSpeed = 1f;
    [SerializeField] private float distanceFromCamera = 1f;

    private LineRenderer currentTrail;
    private List<Vector3> points = new List<Vector3>();

    private void Start()
    {
        InputManager.Instance.OnSwipeStarted += InputManager_OnSwipeStarted;
        InputManager.Instance.OnSwipeEnded += InputManager_OnSwipeEnded;
    }

    private void Update()
    {
        if (currentTrail != null)
        {
            AddPoint();
            UpdateTrailPoints();
            ClearTrailPoints();
        }
    }

    private void InputManager_OnSwipeStarted(object sender, System.EventArgs e)
    {
        CreateTrail();
    }
    private void InputManager_OnSwipeEnded(object sender, System.EventArgs e)
    {
        DestroyTrail();
    }

    private void CreateTrail()
    {
        currentTrail = Instantiate(swipeTrailUIPrefab);
        currentTrail.transform.SetParent(transform, true);
    }

    private void DestroyTrail()
    {
        if (currentTrail != null)
        {
            Destroy(currentTrail.gameObject);
            currentTrail = null;
        }
        points.Clear();
    }

    private void AddPoint()
    {
        Vector3 mousePosition = Input.mousePosition;
        points.Add(Camera.main.ViewportToWorldPoint(new Vector3(mousePosition.x / Screen.width, mousePosition.y / Screen.height, distanceFromCamera)));
    }

    private void UpdateTrailPoints()
    {
        if (currentTrail != null && points.Count > 1)
        {
            currentTrail.positionCount = points.Count;
            currentTrail.SetPositions(points.ToArray());
        }
    }

    private void ClearTrailPoints()
    {
        float clearDistance = Time.deltaTime * clearSpeed;
        while (points.Count > 1 && clearDistance > 0)
        {
            float distance = (points[1] - points[0]).magnitude;
            if (clearDistance > distance)
            {
                points.RemoveAt(0);
            }
            else
            {
                points[0] = Vector3.Lerp(points[0], points[1], clearDistance / distance);
            }
            clearDistance -= distance;
        }
    }
}
