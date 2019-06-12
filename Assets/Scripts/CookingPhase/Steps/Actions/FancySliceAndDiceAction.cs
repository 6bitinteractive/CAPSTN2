using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FancySliceAndDiceAction : Action
{
    // For now, we do this hideous hack
    [SerializeField] private List<InputType> listOfInput;
    [SerializeField] private List<Sprite> ingredientSliceSequence;
    [SerializeField] private Image ingredient;
    [SerializeField] private GameObject swipeDownPrompt;
    [SerializeField] private GameObject swipeRightPrompt;
    [SerializeField] private GameObject tapPrompt;
    [SerializeField] private float delay = 2f;

    private UnityEvent OnTap = new UnityEvent();
    private UnityEvent OnSwipe = new UnityEvent();

    private int currentSequence;
    private InputHandler inputHandler;
    private InputType requiredInput;
    private InputType playerInput;
    private SwipeDirection playerSwipeDirection;

    private void Awake()
    {
        inputHandler = SingletonManager.GetInstance<InputHandler>();
        Active = false;
    }

    private void OnEnable()
    {
        OnTap.AddListener(MoveToNextSlice);
        OnSwipe.AddListener(MoveToNextSlice);
    }

    private void OnDisable()
    {
        OnTap.RemoveAllListeners();
        OnSwipe.RemoveAllListeners();
    }

    private void Update()
    {
        if (!Active) { return; }

        if (requiredInput == InputType.Tap)
        {
            #region Standalone Input
#if UNITY_STANDALONE_WIN
            //Debug.Log("Required to tap");
            if (Input.GetMouseButtonDown(0))
            {
                playerInput = InputType.Tap;
                OnTap.Invoke();
            }
#endif
            #endregion

            #region Mobile Input
#if UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            playerInput = InputType.Tap;
            OnTap.Invoke();

            //switch (touch.phase)
            //{
            //    case TouchPhase.Began:
            //        SetStartPosition(touch.position);
            //        break;

            //    case TouchPhase.Ended:
            //    case TouchPhase.Canceled:
            //        SetEndPosition(touch.position);
            //        BroadcastSwipe();
            //        break;
            //}
        }
#endif
            #endregion
        }
        else if (requiredInput == InputType.SwipeDown || requiredInput == InputType.SwipeRight)
        {
            //Debug.Log("Required to swipe.");
            inputHandler.SwipeDetector.enabled = true;
        }
        else
        {
            //Debug.Log("No input required. Wait for a few seconds");
            StartCoroutine(Pause());
        }
    }

    private IEnumerator Pause()
    {
        yield return new WaitForSeconds(delay);
        playerInput = InputType.None;
        MoveToNextSlice();
    }

    private void CheckSwipe(SwipeData swipeData)
    {
        playerSwipeDirection = swipeData.Direction;

        if (playerSwipeDirection == SwipeDirection.Down || playerSwipeDirection == SwipeDirection.LeftDown || playerSwipeDirection == SwipeDirection.RightDown)
        {
            playerInput = InputType.SwipeDown;
        }
        else if (playerSwipeDirection == SwipeDirection.Right || playerSwipeDirection == SwipeDirection.RightDown || playerSwipeDirection == SwipeDirection.RightUp)
        {
            playerInput = InputType.SwipeRight;
        }

        OnSwipe.Invoke();
    }

    private void MoveToNextSlice()
    {
        inputHandler.SwipeDetector.enabled = false;

        if (playerInput == requiredInput)
        {
            Debug.Log("Correct input");
            currentSequence++;

            ingredient.sprite = ingredientSliceSequence[currentSequence];

            if (currentSequence >= ingredientSliceSequence.Count - 1)
            {
                SwipeDetector.OnSwipe -= CheckSwipe;
                Active = false;
                inputHandler.SwipeDetector.enabled = false;
                Successful = true;
                OnEnd.Invoke(this);
                return;
            }

            ShowInputPrompt();
        }
    }

    private void ShowInputPrompt()
    {
        swipeDownPrompt.SetActive(false);
        swipeRightPrompt.SetActive(false);
        tapPrompt.SetActive(false);

        InputType nextRequiredInput = listOfInput[currentSequence + 1];
        requiredInput = nextRequiredInput;
        Debug.Log("Required input: " + nextRequiredInput);

        switch (nextRequiredInput)
        {
            case InputType.SwipeDown:
                swipeDownPrompt.SetActive(true);
                break;
            case InputType.SwipeRight:
                swipeRightPrompt.SetActive(true);
                break;
            case InputType.Tap:
                tapPrompt.SetActive(true);
                break;
        }
    }

    public override void Begin()
    {
        SwipeDetector.OnSwipe += CheckSwipe;
        OnBegin.Invoke(this);
        inputHandler.SwipeDetector.enabled = false;
        Active = true;
        currentSequence = 0;
        requiredInput = listOfInput[currentSequence + 1];
        ShowInputPrompt();
    }

    public override void ResetAction()
    {
        Active = false;
    }

    public override bool SuccessConditionMet()
    {
        throw new System.NotImplementedException();
    }
}
