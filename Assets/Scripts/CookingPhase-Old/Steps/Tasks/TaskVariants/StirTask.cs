using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StirTask : Task
{
    [SerializeField] private int stirCountRequired = 2;
    public UnityEvent OnStir = new UnityEvent();

    private CircleGestureDetector circleGestureDetector;
    private int currentStirCount;

    protected override void Setup()
    {
        //circleGestureDetector = inputHandler.CircleGestureDetector;
        circleGestureDetector.enabled = true;
        circleGestureDetector.OnClockwiseCircleGesture.AddListener(InvokeOnStir);
    }

    protected override void FinalizeTask()
    {
        circleGestureDetector.enabled = false;
        circleGestureDetector.OnClockwiseCircleGesture.RemoveListener(InvokeOnStir);
    }

    protected override bool SuccessConditionMet()
    {
        return currentStirCount >= stirCountRequired;
    }

    private void InvokeOnStir()
    {
        currentStirCount++;
        OnStir.Invoke();
    }
}
