using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LidTask : Task
{
    [SerializeField] private GameObject lid;
    [SerializeField] private State targetState;

    private static string takeOff = "TakeOff";
    private static string putOn = "PutOn";

    public UnityEvent OnLidOn = new UnityEvent();
    public UnityEvent OnLidOff = new UnityEvent();

    private SwipeDetector swipeDetector;
    private Animator animator;
    private State currentState;

    protected override void Setup()
    {
        animator = lid.GetComponent<Animator>();
        swipeDetector = inputHandler.SwipeDetector;
        swipeDetector.enabled = true;
        SwipeDetector.OnSwipe += CheckInput;
    }

    protected override void FinalizeTask()
    {
        swipeDetector.enabled = false;
        SwipeDetector.OnSwipe -= CheckInput;
    }

    protected override bool SuccessConditionMet()
    {
        return currentState == targetState;
    }

    private void CheckInput(SwipeData swipeData)
    {
        if (!Active) { return; }

        switch (targetState)
        {
            case State.PutOn:
                if (swipeData.Direction == SwipeDirection.Down
                    || swipeData.Direction == SwipeDirection.LeftDown
                    || swipeData.Direction == SwipeDirection.RightDown)
                {
                    animator.SetTrigger(putOn);
                    currentState = State.PutOn;
                    OnLidOn.Invoke();
                }
                break;
            case State.TakeOff:
                if (swipeData.Direction == SwipeDirection.Up
                    || swipeData.Direction == SwipeDirection.LeftUp
                    || swipeData.Direction == SwipeDirection.RightUp)
                {
                    animator.SetTrigger(takeOff);
                    currentState = State.TakeOff;
                    OnLidOff.Invoke();
                }
                break;
            default:
                Debug.LogError("Can't put on or take off the lid");
                break;
        }
    }

    public enum State
    {
        PutOn,
        TakeOff
    }
}
