using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LidAction : Action
{
    [SerializeField] private GameObject lid;
    [SerializeField] private State targetState;
    [SerializeField] private float waitDuration = 3f;

    private static string takeOff = "TakeOff";
    private static string putOn = "PutOn";

    public UnityEvent OnLidOn = new UnityEvent();
    public UnityEvent OnLidOff = new UnityEvent();

    private InputHandler inputHandler;
    private Animator animator;
    private State currentState;

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
        inputHandler.SwipeInput.Clear();
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
                    OnLidOn.Invoke();
                }
                break;
            case State.TakeOff:
                if (inputHandler.SwipeInput.ContainsKey(SwipeDirection.Up)
                    || inputHandler.SwipeInput.ContainsKey(SwipeDirection.LeftUp)
                    || inputHandler.SwipeInput.ContainsKey(SwipeDirection.RightUp))
                {
                    animator.SetTrigger(takeOff);
                    currentState = State.TakeOff;
                    OnLidOff.Invoke();
                }
                break;
            default:
                Debug.Log("Can't put on or take off the lid");
                break;
        }

        inputHandler.SwipeDetector.enabled = false;
        SuccessConditionMet();
        Active = false;
        StopAllCoroutines();
        StartCoroutine(End());
    }

    private IEnumerator End()
    {
        yield return new WaitForSeconds(waitDuration);
        OnEnd.Invoke(this);
    }

    public override bool SuccessConditionMet()
    {
        Successful = currentState == targetState;
        Debug.Log(gameObject.name + " sucessful: " + Successful);

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
