using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SwipeAreaPrompt : Prompt, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private RectTransform rectTransform;
    private GameObject parent;
    private SwipeDirection arrowPromptDirection = SwipeDirection.None;
    private bool inputInCorrectArea;
    private SwipeDirectionListener swipeDirectionListener;

    public UnityEvent OnCorrectSwipe = new UnityEvent();
    public SwipeEvent OnCorrectSwipeDirection = new SwipeEvent();

    protected override void Awake()
    {
        base.Awake();

        rectTransform = GetComponent<RectTransform>();
        parent = transform.parent.gameObject;
        arrowPromptDirection = GetComponentInParent<ArrowPrompt>().SwipeDirection;
        if (arrowPromptDirection == SwipeDirection.None)
            Debug.LogError("No ArrowPrompt component found or direction has not been set properly.");
    }

    private void OnEnable()
    {
        swipeDirectionListener = SingletonManager.GetInstance<SwipeDirectionListener>();
        SwipeDetector.OnSwipe += VerifySwipe;

        OnCorrectSwipeDirection.AddListener(swipeDirectionListener.InvokeOnCorrectSwipe);
    }

    private void OnDisable()
    {
        inputInCorrectArea = false;
        SwipeDetector.OnSwipe -= VerifySwipe;

        OnCorrectSwipeDirection.RemoveListener(swipeDirectionListener.InvokeOnCorrectSwipe);
    }

    private void VerifySwipe(SwipeData swipeData)
    {
        if (swipeData.Direction == SwipeDirection.None)
            return;

        if (RectTransformUtility.RectangleContainsScreenPoint(rectTransform, swipeData.StartPosition))
        {
            inputInCorrectArea = true;
        }

        if (swipeData.Direction == arrowPromptDirection && inputInCorrectArea)
        {
            OnCorrectSwipeDirection.Invoke(swipeData);
            OnCorrectSwipe.Invoke();
            parent.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //inputInCorrectArea = true;
        //Debug.Log("Within correct area");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //inputInCorrectArea = false;
        //Debug.Log("Outside correct area");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //inputInCorrectArea = true;
        //Debug.Log("Within correct area");
    }
}
