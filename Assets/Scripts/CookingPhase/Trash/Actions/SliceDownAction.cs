﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliceDownAction : Action
{
    [SerializeField] private Sprite[] ingredientSliceSequence;
    [SerializeField] private Image ingredient;
    private InputHandler inputHandler;

    private int sliceCount;

    public override void Begin()
    {
        inputHandler.SwipeDetector.enabled = true;
        Active = true;
        OnBegin.Invoke(this);
    }

    public override void ResetAction()
    {
        sliceCount = 0;
    }

    public override bool SuccessConditionMet()
    {
        return Successful;
    }

    private void OnEnable()
    {
        SwipeDetector.OnSwipe += CheckInput;
    }

    private void OnDisable()
    {
        SwipeDetector.OnSwipe -= CheckInput;
    }

    private void Awake()
    {
        inputHandler = SingletonManager.GetInstance<InputHandler>();
    }

    private void CheckInput(SwipeData swipeData)
    {
        if (!Active) { return; }

        if (sliceCount < ingredientSliceSequence.Length)
        {
            // If the player swiped down
            if (inputHandler.SwipeInput.ContainsKey(SwipeDirection.Down)
                || inputHandler.SwipeInput.ContainsKey(SwipeDirection.LeftDown)
                || inputHandler.SwipeInput.ContainsKey(SwipeDirection.RightDown))
            {
                // Increase slice count
                sliceCount++;

                // Show the ingredient as sliced
                ingredient.sprite = ingredientSliceSequence[sliceCount];

                // Check if it's done
                if (sliceCount == ingredientSliceSequence.Length - 1)
                {
                    Active = false;
                    inputHandler.SwipeInput.Clear();
                    inputHandler.SwipeDetector.enabled = false;
                    Successful = true;
                    OnEnd.Invoke(this);
                }

                // Clear the swipe input container
                inputHandler.SwipeInput.Clear();
            }
        }
    }
}