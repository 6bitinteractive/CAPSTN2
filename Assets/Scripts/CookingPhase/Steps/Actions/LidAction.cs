using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LidAction : Action
{
    [SerializeField] private GameObject lid;
    [SerializeField] private State targetState;

    [Header("Animator")]
    [SerializeField] private string takeOff = "TakeOff";
    [SerializeField] private string putOn = "PutOn";

    public UnityEvent OnLidOn = new UnityEvent();
    public UnityEvent OnLidOff = new UnityEvent();

    private InputHandler inputHandler;
    private Animator animator;
    private State currentState;

    private float timer;

    private void Awake()
    {
        inputHandler = SingletonManager.GetInstance<InputHandler>();
        animator = lid.GetComponent<Animator>();
    }

    private void OnEnable()
    {
        SwipeDetector.OnSwipe += CheckInput;
    }

    private void OnDisable()
    {
        SwipeDetector.OnSwipe -= CheckInput;
    }

    public override void ResetAction()
    {
        lid.SetActive(false);
        inputHandler.SwipeDetector.enabled = false;
    }

    public override void Begin()
    {
        OnBegin.Invoke(this);
        Active = true;
        inputHandler.SwipeDetector.enabled = true;
        lid.SetActive(true);
    }

    private void CheckInput(SwipeData swipeData)
    {
        if (!Active) { return; }

        switch (targetState)
        {
            case State.PutOn:
                if (inputHandler.SwipeInput.ContainsKey(SwipeDirection.Down)
                    || inputHandler.SwipeInput.ContainsKey(SwipeDirection.LeftDown)
                    || inputHandler.SwipeInput.ContainsKey(SwipeDirection.RightDown))
                {
                    animator.SetTrigger(putOn);
                    currentState = State.PutOn;
                    inputHandler.SwipeInput.Clear();
                    inputHandler.SwipeDetector.enabled = false;
                    OnLidOn.Invoke();
                    OnEnd.Invoke(this);
                }
                break;
            case State.TakeOff:
                if (inputHandler.SwipeInput.ContainsKey(SwipeDirection.Up)
                    || inputHandler.SwipeInput.ContainsKey(SwipeDirection.LeftUp)
                    || inputHandler.SwipeInput.ContainsKey(SwipeDirection.RightUp))
                {
                    animator.SetTrigger(takeOff);
                    currentState = State.TakeOff;
                    inputHandler.SwipeInput.Clear();
                    inputHandler.SwipeDetector.enabled = false;
                    OnLidOff.Invoke();
                    OnEnd.Invoke(this);
                }
                break;
            default:
                Debug.Log("Can't put on or take off the lid");
                break;
        }
    }

    public override bool SuccessConditionMet()
    {
        Successful = targetState != currentState;

        // TODO: rewrite; take this out of here
        if (Successful)
            OnSuccess.Invoke(this);
        else
            OnFail.Invoke(this);

        return Successful;
    }

    public enum State
    {
        PutOn,
        TakeOff
    }
}
