﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public Dictionary<SwipeDirection, SwipeData> SwipeInput = new Dictionary<SwipeDirection, SwipeData>();
    [HideInInspector] public SwipeDetector SwipeDetector;

    private void Awake()
    {
        SingletonManager.Register<InputHandler>(this);
        SwipeDetector = GetComponent<SwipeDetector>();
        SwipeDetector.enabled = false;
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

        if (!SwipeInput.ContainsKey(swipeData.Direction) || SwipeInput.Count < 1)
        {
            SwipeInput.Add(swipeData.Direction, swipeData);
    }
}
}
