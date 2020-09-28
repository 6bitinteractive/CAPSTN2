using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CircleGestureDetector : MonoBehaviour
{
    [SerializeField] private int numOfCircleToShow = 1;

    List<Vector2> gestureDetector = new List<Vector2>();
    Vector2 gestureSum = Vector2.zero;
    float gestureLength = 0;
    int gestureCount = 0;

    [HideInInspector] public UnityEvent OnClockwiseCircleGesture = new UnityEvent();

    private void Update()
    {
        if (IsGestureDone())
            Debug.Log("Circle gesture detected.");
    }

    private bool IsGestureDone()
    {

       // #region Standalone Input
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
        if (Input.GetMouseButtonUp(0))
        {
            gestureDetector.Clear();
            gestureCount = 0;
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                Vector2 p = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                if (gestureDetector.Count == 0 || (p - gestureDetector[gestureDetector.Count - 1]).magnitude > 10)
                {
                    Debug.Log(gestureDetector.Count);
                    gestureDetector.Add(p);
                }
            }
        }

        // #region Mobile Input
#elif UNITY_ANDROID || UNITY_IOS
        if (Input.touches.Length != 1)
        {
            Debug.Log("test");
            gestureDetector.Clear();
            gestureCount = 0;
        }
        else
        {
            if (Input.touches[0].phase == TouchPhase.Canceled || Input.touches[0].phase == TouchPhase.Ended)
                gestureDetector.Clear();
            else if (Input.touches[0].phase == TouchPhase.Moved)
            {
                Vector2 p = Input.touches[0].position;
                if (gestureDetector.Count == 0 || (p - gestureDetector[gestureDetector.Count - 1]).magnitude > 10)
                    gestureDetector.Add(p);
            }
        }
#endif
        //#endregion

        if (gestureDetector.Count < 5)
            return false;

        gestureSum = Vector2.zero;
        gestureLength = 0;
        Vector2 prevDelta = Vector2.zero;
        for (int i = 0; i < gestureDetector.Count - 2; i++)
        {

            Vector2 delta = gestureDetector[i + 1] - gestureDetector[i];
            float deltaLength = delta.magnitude;
            gestureSum += delta;
            gestureLength += deltaLength;

            float dot = Vector2.Dot(delta, prevDelta);
            if (dot < 0f)
            {
                gestureDetector.Clear();
                gestureCount = 0;
                return false;
            }

            prevDelta = delta;
        }

        int gestureBase = (Screen.width + Screen.height) / 4;

        // Is it clockwise
        float orientation = GetOrientation(gestureDetector[0], gestureDetector[1], gestureDetector[2]);
        //Debug.Log("Orientation: " + orientation);

        if (gestureLength > gestureBase && gestureSum.magnitude < gestureBase / 2)
        {
            gestureDetector.Clear();
            gestureCount++;
            if (gestureCount >= numOfCircleToShow)
            {
                if (orientation == 1)
                {
                    OnClockwiseCircleGesture.Invoke();
                    return true;
                }

                return true;
            }
        }

        return false;
    }

    private float GetOrientation(Vector2 p1, Vector2 p2, Vector2 p3)
    {
        float orientation = (p2.y - p1.y) * (p3.x - p2.x) - (p2.x - p1.x) * (p3.y - p2.y);

        if (orientation == 0) return 0; // colinear

        // clock or counterclock wise
        return (orientation > 0) ? 1 : 2;
    }
}

// Source: https://github.com/aliessmael/Unity-Logs-Viewer/blob/master/Reporter/Reporter.cs
// https://www.geeksforgeeks.org/orientation-3-ordered-points/
