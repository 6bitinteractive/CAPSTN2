using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Swipeable : MonoBehaviour
{
    public SwipeDirection SwipeDirection;
    public UnityEvent OnCorrectSwipe = new UnityEvent();
    public SwipeEvent OnCorrectSwipeDirection = new SwipeEvent();

    private SwipeDirectionListener swipeDirectionListener;
    private bool inputInCorrectArea;
    private RectTransform rectTransform;

    private void OnEnable()
    {
        swipeDirectionListener = SingletonManager.GetInstance<SwipeDirectionListener>();
        SwipeDetector.OnSwipe += VerifySwipe;
        OnCorrectSwipeDirection.AddListener(swipeDirectionListener.InvokeOnCorrectSwipe);
    }

    private void VerifySwipe(SwipeData swipeData)
    {
        Debug.Log("Nep");
        if (swipeData.Direction == SwipeDirection.None)
            return;

        if (RectTransformUtility.RectangleContainsScreenPoint(rectTransform, swipeData.StartPosition))
        {
            inputInCorrectArea = true;
            Debug.Log("a");
        }

        if (swipeData.Direction == SwipeDirection)
        {
            OnCorrectSwipeDirection.Invoke(swipeData);
            OnCorrectSwipe.Invoke();
            Debug.Log("b");
        }
    }
}
