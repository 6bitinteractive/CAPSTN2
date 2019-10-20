using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]

public class SwipeRenderer : MonoBehaviour
{
    [SerializeField] private float clearDelayTime = 1f;
    [SerializeField] private float zOffset = 10f;

    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void OnEnable()
    {
        SwipeDetector.OnSwipe += RenderSwipe;
    }

    private void OnDisable()
    {
        SwipeDetector.OnSwipe -= RenderSwipe;
    }

    private void RenderSwipe(SwipeData data)
    {
        Vector3[] positions = new Vector3[2];
        positions[0] = Camera.main.ScreenToWorldPoint(new Vector3(data.StartPosition.x, data.StartPosition.y, zOffset));
        positions[1] = Camera.main.ScreenToWorldPoint(new Vector3(data.EndPosition.x, data.EndPosition.y, zOffset));

        lineRenderer.positionCount = 2;
        lineRenderer.SetPositions(positions);

        Invoke("ClearLineRender", clearDelayTime);
    }

    private void ClearLineRender()
    {
        Vector3[] positions = new Vector3[2];
        positions[0] = Vector3.zero;
        positions[1] = Vector3.zero;

        lineRenderer.positionCount = 2;
        lineRenderer.SetPositions(positions);
    }
}
