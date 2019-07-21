using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable] public class SwipeEvent : UnityEvent<SwipeData> { }

public class SwipeDetector : MonoBehaviour
{
    [SerializeField] private float minimumHorizontalSwipeDistance = 20f;
    [SerializeField] private float minimumVerticalSwipeDistance = 20f;

    [Tooltip("Enabling this would allow for a more stricter check, i.e. a diagonal swipe will not be acknowledged as a correct left/right swipe.")]
    [SerializeField] private bool supportDiagonalDetection = false;

    private Vector2 startPosition;
    private Vector2 endPosition;
    private Vector2 DeltaSwipe { get { return startPosition - endPosition; } }

    public static event Action<SwipeData> OnSwipe = delegate { };

    private void Update()
    {
        #region Standalone Input
#if UNITY_STANDALONE_WIN
        if (Input.GetMouseButtonDown(0))
        {
            SetStartPosition(Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(0))
        {
            SetEndPosition(Input.mousePosition);
            BroadcastSwipe();
        }
#endif
        #endregion

        #region Mobile Input
#if UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    SetStartPosition(touch.position);
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    SetEndPosition(touch.position);
                    BroadcastSwipe();
                    break;
            }
        }
#endif
        #endregion
    }

    private void SetStartPosition(Vector2 position)
    {
        endPosition = startPosition = position; // The endPosition is also at the startPosition at the beginning
    }

    private void SetEndPosition(Vector2 position)
    {
        endPosition = position;
    }

    private SwipeDirection DetermineSwipeDirection()
    {
        SwipeDirection swipeDirection = SwipeDirection.None;
        float horizontalSwipeDistance = Mathf.Abs(DeltaSwipe.x);
        float verticalSwipeDistance = Mathf.Abs(DeltaSwipe.y);
        bool minHorizontalDistanceMet = horizontalSwipeDistance > minimumHorizontalSwipeDistance;
        bool minVerticalDistanceMet = verticalSwipeDistance > minimumVerticalSwipeDistance;

        if (supportDiagonalDetection)
        {
            if (minHorizontalDistanceMet)
                swipeDirection |= (DeltaSwipe.x < 0) ? SwipeDirection.Right : SwipeDirection.Left;

            if (minVerticalDistanceMet)
                swipeDirection |= (DeltaSwipe.y < 0) ? SwipeDirection.Up : SwipeDirection.Down;
        }
        else
        {
            if (minHorizontalDistanceMet || minVerticalDistanceMet)
            {
                bool isVerticalSwipe = verticalSwipeDistance > horizontalSwipeDistance;

                if (isVerticalSwipe)
                    swipeDirection = (DeltaSwipe.y < 0) ? SwipeDirection.Up : SwipeDirection.Down;
                else
                    swipeDirection = (DeltaSwipe.x < 0) ? SwipeDirection.Right : SwipeDirection.Left;
            }
        }

        return swipeDirection;
    }

    private void BroadcastSwipe()
    {
        SwipeData swipeData = new SwipeData()
        {
            StartPosition = startPosition,
            EndPosition = endPosition,
            Direction = DetermineSwipeDirection()
        };

        OnSwipe(swipeData);
    }
}

public enum SwipeDirection
{ // Bit flags
    None = 0,
    Left = 1,
    Right = 2,
    Up = 4,
    Down = 8,

    // Diagonal
    LeftUp = 5,
    RightUp = 6,
    LeftDown = 9,
    RightDown = 10
}

public struct SwipeData
{
    public Vector2 StartPosition;
    public Vector2 EndPosition;
    public SwipeDirection Direction;
}

// Reference:
// https://www.youtube.com/watch?v=poeXGuQ7eUo
// https://www.youtube.com/watch?v=jbFYYbu5bdc
// |= operator https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/or-assignment-operator
