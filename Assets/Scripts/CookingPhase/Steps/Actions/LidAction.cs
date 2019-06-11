using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LidAction : Action
{
    [SerializeField] private GameObject lid;

    [Tooltip("If false, lid will be put on the pot.")]
    [SerializeField] private bool takeOffLid;

    [Header("Animator")]
    [SerializeField] private string takeOff = "TakeOff";
    [SerializeField] private string putOn = "PutOn";

    private InputHandler inputHandler;
    private Animator animator;

    public UnityEvent OnLidOn = new UnityEvent();

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
        inputHandler.enabled = false;
        inputHandler.SwipeDetector.enabled = false;
    }

    public override void Begin()
    {
        Active = true;
        inputHandler.enabled = true;
        inputHandler.SwipeDetector.enabled = true;
        lid.SetActive(true);
    }

    private void CheckInput(SwipeData swipeData)
    {
        if (!Active) { return; }

        if (takeOffLid)
        {
            if (inputHandler.SwipeInput.ContainsKey(SwipeDirection.Up)
                || inputHandler.SwipeInput.ContainsKey(SwipeDirection.LeftUp)
                || inputHandler.SwipeInput.ContainsKey(SwipeDirection.RightUp))
            {
                animator.SetTrigger(takeOff);
                Active = false;
            }
        }
        else
        {
            if (inputHandler.SwipeInput.ContainsKey(SwipeDirection.Down)
                || inputHandler.SwipeInput.ContainsKey(SwipeDirection.LeftDown)
                || inputHandler.SwipeInput.ContainsKey(SwipeDirection.RightDown))
            {
                animator.SetTrigger(putOn);
                OnLidOn.Invoke();
                Active = false;
            }
        }
    }
}
