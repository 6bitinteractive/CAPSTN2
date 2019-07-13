using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SwipeAreaPrompt : Prompt, IPointerEnterHandler, IPointerExitHandler
{
    private GameObject parent;
    private SwipeDirection arrowPromptDirection = SwipeDirection.None;
    private bool inputInCorrectArea;

    protected override void Awake()
    {
        base.Awake();

        parent = transform.parent.gameObject;
        arrowPromptDirection = GetComponentInParent<ArrowPrompt>().SwipeDirection;
        if (arrowPromptDirection == SwipeDirection.None)
            Debug.LogError("No ArrowPrompt component found or direction has not been set properly.");
    }

    private void OnEnable()
    {
        SwipeDetector.OnSwipe += VerifySwipe;
    }

    private void OnDisable()
    {
        SwipeDetector.OnSwipe -= VerifySwipe;
    }

    private void VerifySwipe(SwipeData swipeData)
    {
        if (swipeData.Direction == SwipeDirection.None)
            return;

        if (swipeData.Direction == arrowPromptDirection && inputInCorrectArea)
            OnSuccessfulInput.Invoke();
        else
            OnFailedInput.Invoke();

        parent.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        inputInCorrectArea = true;
        Debug.Log("Within correct area");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        inputInCorrectArea = false;
        Debug.Log("Outside correct area");
    }
}
