using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public Dictionary<SwipeDirection, SwipeData> SwipeInput = new Dictionary<SwipeDirection, SwipeData>();
    [HideInInspector] public SwipeDetector SwipeDetector;
    [HideInInspector] public CircleGestureDetector CircleGestureDetector;

    private void Awake()
    {
        SingletonManager.Register<InputHandler>(this);

        SwipeDetector = GetComponent<SwipeDetector>();
        SwipeDetector.enabled = false;

        CircleGestureDetector = GetComponent<CircleGestureDetector>();
        CircleGestureDetector.enabled = false;
    }

    private void OnEnable()
    {
        SwipeDetector.OnSwipe += OnSwipe;
    }

    private void OnDisable()
    {
        SwipeDetector.OnSwipe -= OnSwipe;
    }

    private void OnSwipe(SwipeData swipeData)
    {
        Debug.Log("Swipe: " + swipeData.Direction);

        if (!SwipeInput.ContainsKey(swipeData.Direction))
        {
            SwipeInput.Add(swipeData.Direction, swipeData);
        }
    }
}

public enum InputType
{
    None,
    SwipeDown,
    SwipeRight,
    Tap
}
