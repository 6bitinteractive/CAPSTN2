using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SwipeDetector))]

public class TransitionalStepObjective : Objective
{
    [SerializeField] private Animator cookwareAnimator;
    [SerializeField] private TransitionType transitionType;
    [SerializeField] private DialogueHint dialogueHintOnFail;
    [SerializeField] private Animator swipePrompt;
    [SerializeField] private ObjectiveState perfectState = new ObjectiveState(ObjectiveState.Status.Perfect);

    private SwipeDetector swipeDetector;
    private DialogueHintManager dialogueHintManager;
    private bool success;

    protected override void Awake()
    {
        base.Awake();
        swipeDetector = GetComponent<SwipeDetector>();
        swipeDetector.enabled = false;

        // Setup objectives
        // Add to list
        ObjectiveStates.Add(perfectState);

        // Define condition
        perfectState.HasBeenReached = () => SuccessConditionMet();
    }

    protected override void InitializeObjective()
    {
        base.InitializeObjective();
        dialogueHintManager = SingletonManager.GetInstance<DialogueHintManager>();
        perfectState.OnStateReached.AddListener((x) => swipePrompt.gameObject.SetActive(false));

        // Show prompt
        swipePrompt.gameObject.SetActive(true);
        if (transitionType == TransitionType.Entrance)
            swipePrompt.SetTrigger("SwipeLeft");
        else
            swipePrompt.SetTrigger("SwipeRight");

        // Allow to go to next step
        GoToNextObjective(false);

        swipeDetector.enabled = true;
        SwipeDetector.OnSwipe += Transition;
    }

    protected override void GoToNextObjective(bool automatic = false)
    {
        base.GoToNextObjective(automatic);

        if (automatic)
            swipeDetector.enabled = false;
    }

    protected override void PostFinalizeObjective()
    {
        base.PostFinalizeObjective();
        SwipeDetector.OnSwipe -= Transition;

        if (!success)
        {
            if (transitionType == TransitionType.Entrance)
                cookwareAnimator.SetTrigger("SlideIn");
            else
                cookwareAnimator.SetTrigger("SlideOut");
        }

        swipePrompt.ResetTrigger("SlideIn");
        swipePrompt.ResetTrigger("SlideOut");
        swipePrompt.gameObject.SetActive(false);
    }

    protected override bool SuccessConditionMet()
    {
        return success;
    }

    private void Transition(SwipeData swipeData)
    {
        StartCoroutine(OnTransition(swipeData));
    }

    private IEnumerator OnTransition(SwipeData swipeData)
    {
        if (success) yield break;

        Debug.Log("Swiped: " + swipeData.Direction);
        if ((swipeData.Direction == SwipeDirection.Left
            || swipeData.Direction == SwipeDirection.LeftDown
            || swipeData.Direction == SwipeDirection.LeftUp)
            && transitionType == TransitionType.Entrance)
        {
            success = true;
            cookwareAnimator.SetTrigger("SlideIn");
            yield return new WaitUntil(() => AnimatorUtils.IsDonePlaying(cookwareAnimator, "CookwareSlideIn"));

            yield return new WaitForSeconds(1f);
            GoToNextObjective(true);
        }
        else if ((swipeData.Direction == SwipeDirection.Right
            || swipeData.Direction == SwipeDirection.RightDown
            || swipeData.Direction == SwipeDirection.RightUp)
            && transitionType == TransitionType.Exit)
        {

            success = true;
            cookwareAnimator.SetTrigger("SlideOut");
            yield return new WaitUntil(() => AnimatorUtils.IsDonePlaying(cookwareAnimator, "CookwareSlideOut"));

            // Wait for a bit so that items that are to be disabled are done outside the screen
            yield return new WaitForSeconds(1f);
            GoToNextObjective(true);
        }
        else
        {
            dialogueHintManager.Show(dialogueHintOnFail);
            success = false;
        }

    }

    public enum TransitionType
    {
        Entrance,
        Exit
    }
}
