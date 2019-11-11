using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Note: Make sure the object attached with the SpoonKitchenUtensil does NOT have collision check enabled with the layer for ingredients
// And IsTrigger is set to false
// It somehow messes up with the colliders for checking if it's in the cookware???

public class StirFryObjective : Objective
{
    [SerializeField] private SpoonKitchenUtensil spoon;
    [SerializeField] private float durationLimit = 10f;
    [SerializeField] private ProgressMeter progressMeter;
    [SerializeField] private float nextButtonDisplayDelay = 2f;
    [SerializeField] private DialogueHint dialogueHint;
    [SerializeField] private ObjectiveState perfectState = new ObjectiveState(ObjectiveState.Status.Perfect);
    [SerializeField] private ObjectiveState underState = new ObjectiveState(ObjectiveState.Status.Under);
    [SerializeField] private ObjectiveState overState = new ObjectiveState(ObjectiveState.Status.Over);

    private float minDuration;
    private float maxDuration;
    private Animator spoonAnim;
    private DisableAnimation spoonDisableAnim;
    private bool showNextButton = true;
    private List<IngredientStateController> ingredients = new List<IngredientStateController>();

    protected override void Awake()
    {
        base.Awake();
        minDuration = durationLimit * progressMeter.perfectMin;
        maxDuration = durationLimit * progressMeter.perfectMax;

        spoonAnim = spoon.GetComponent<Animator>();
        spoonDisableAnim = spoon.GetComponent<DisableAnimation>();

        // Setup objectives
        // Add to list
        ObjectiveStates.Add(perfectState);
        ObjectiveStates.Add(underState);
        ObjectiveStates.Add(overState);

        // Define condition
        perfectState.HasBeenReached = () => SuccessConditionMet();
        underState.HasBeenReached = () => spoon.MixDuration < minDuration && spoon.MixDuration > 2f;
        overState.HasBeenReached = () => spoon.MixDuration > maxDuration;
    }

    protected override void InitializeObjective()
    {
        base.InitializeObjective();
        SingletonManager.GetInstance<DialogueHintManager>().Show(dialogueHint);
        progressMeter.transform.parent.gameObject.SetActive(true);
        spoon.enabled = false; // Turn off at start so that movement when spoon enters the pan at start is not counted as mixing

        if (spoon.gameObject.activeInHierarchy)
        {
            spoonDisableAnim.enabled = true;
            spoonAnim.SetTrigger("SlideIn");
        }
        else
        {
            spoon.gameObject.SetActive(true);
        }

        foreach (var item in kitchen.Cookware.CookableIngredients)
        {
            ingredients.AddRange(item.IngredientInCookware.GetComponentsInChildren<IngredientStateController>());
            Debug.Log("Total ingredients: " + ingredients.Count);
        }

        perfectState.OnStateReached.AddListener(SwitchState);
        overState.OnStateReached.AddListener(SwitchState);

        // Enable spoon only after its slide in animation
        StartCoroutine(EnableSpoon());
    }

    protected override void RunObjective()
    {
        base.RunObjective();

        // Progress Meter
        if (durationLimit <= 0f)
            Debug.Log("Duration limit cannot be less than or equal to zero.");

        if (spoon.IsMixing)
        {
            float progressValue = spoon.MixDuration/ durationLimit;
            progressMeter.UpdateProgress(progressValue);
        }

        // If player has mixed for a few seconds, show the Next button if it hasn't been shown yet
        if (spoon.MixDuration >= nextButtonDisplayDelay && showNextButton)
        {
            showNextButton = false;
            GoToNextObjective(false);
        }
    }

    protected override void PostFinalizeObjective()
    {
        base.FinalizeObjective();
        spoon.MixDuration = 0f;
        progressMeter.transform.parent.gameObject.SetActive(false);
    }

    protected override bool SuccessConditionMet()
    {
        return spoon.MixDuration >= minDuration && spoon.MixDuration <= maxDuration;
    }

    private void SwitchState(ObjectiveState objectiveState)
    {
        IngredientState state = IngredientState.Raw;
        if (objectiveState.StatusType == ObjectiveState.Status.Perfect)
        {
            state = IngredientState.Perfect;
        }
        else if (objectiveState.StatusType == ObjectiveState.Status.Over)
        {
            state = IngredientState.Overcooked;
        }

        foreach (var item in ingredients)
        {
            item.SwitchState(state);
        }
    }

    private IEnumerator EnableSpoon()
    {
        yield return new WaitUntil(() => AnimatorUtils.IsInState(spoonAnim, "SlideIn") && AnimatorUtils.IsDonePlaying(spoonAnim, "SlideIn"));
        yield return new WaitForSeconds(1f); // Wait for a bit so that movement within the pan when the spoon slides in doesn't count as valid input/mixing
        spoon.enabled = true;
    }
}
