using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SwipeDetector))]

public class RemoveScumObjective : Objective
{
    [SerializeField] private WaterScum waterScum;
    [SerializeField] private KitchenUtensil ladle;
    [SerializeField] private DialogueHint dialogueHint;

    [SerializeField] private ObjectiveState perfectState = new ObjectiveState(ObjectiveState.Status.Perfect);
    [SerializeField] private ObjectiveState underState = new ObjectiveState(ObjectiveState.Status.Under);

    private SwipeDetector swipeDetector;
    private Animator ladleAnimator;
    private int scumRemovedCount;

    protected override void Awake()
    {
        base.Awake();
        swipeDetector = GetComponent<SwipeDetector>();
        ladleAnimator = ladle.GetComponent<Animator>();

        // Setup objectives
        // Add to list
        ObjectiveStates.Add(perfectState);
        ObjectiveStates.Add(underState);

        // Define condition
        perfectState.HasBeenReached = () => waterScum.Removed; // Player has removed all of the scum
        underState.HasBeenReached = () => scumRemovedCount == 1; // Encourage player to remove more after first swipe
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        swipeDetector.enabled = true;
        SwipeDetector.OnSwipe += VerifySwipe;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        swipeDetector.enabled = false;
        SwipeDetector.OnSwipe -= VerifySwipe;
    }

    protected override void InitializeObjective()
    {
        base.InitializeObjective();

        // Show the ladle
        ladleAnimator.SetTrigger("SlideIn");

        // Show the dialogue hint
        SingletonManager.GetInstance<DialogueHintManager>().Show(dialogueHint);

        // Listen to event
        underState.OnStateReached.AddListener(x => GoToNextObjective(false));
    }

    protected override void FinalizeObjective()
    {
        base.FinalizeObjective();
        ladleAnimator.SetTrigger("SlideOut");
    }

    protected override bool SuccessConditionMet()
    {
        return waterScum.Removed;
    }

    private void VerifySwipe(SwipeData swipeData)
    {
        switch (swipeData.Direction)
        {
            case SwipeDirection.Left:
            case SwipeDirection.LeftUp:
            case SwipeDirection.LeftDown:
                if (ladle.InCookware)
                {
                    if (waterScum.Removed) { return; }
                    waterScum.Remove();
                    ladleAnimator.SetTrigger("Scoop");
                    scumRemovedCount++;
                }
                break;
            default:
                break;
        }
    }
}
