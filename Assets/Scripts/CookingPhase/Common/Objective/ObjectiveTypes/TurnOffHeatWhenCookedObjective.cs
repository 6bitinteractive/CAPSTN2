using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnOffHeatWhenCookedObjective : Objective
{
    [SerializeField] private GameObject panLid;
    [SerializeField] private Cookware cookware;
    [SerializeField] private Slider stoveControllerSlider;
    [SerializeField] private DialogueHint dialogueHint;
    [SerializeField] private DialogueHint failHeatOffDialogue;

    [SerializeField] private ObjectiveState perfectState = new ObjectiveState(ObjectiveState.Status.Perfect);
    [SerializeField] private ObjectiveState underState = new ObjectiveState(ObjectiveState.Status.Under);
    [SerializeField] private ObjectiveState overState = new ObjectiveState(ObjectiveState.Status.Over);

    private List<IngredientStateController> ingredients = new List<IngredientStateController>();
    private StoveController stoveController;
    private Animator stoveControllerAnimator;
    private LidHandler lid;
    private Animator lidAnimator;
    private bool showStoveControllerHint = true;

    protected override void Awake()
    {
        base.Awake();
        stoveController = stoveControllerSlider.GetComponent<StoveController>();
        stoveControllerAnimator = stoveController.GetComponent<Animator>();
        lid = panLid.GetComponent<LidHandler>();
        lidAnimator = lid.GetComponent<Animator>();

        // Setup objectives
        // Add to list
        ObjectiveStates.Add(perfectState);
        ObjectiveStates.Add(underState);
        ObjectiveStates.Add(overState);

        // Define condition
        perfectState.HasBeenReached = () => ingredients.Exists(x => x.CurrentState == IngredientState.Perfect);
        underState.HasBeenReached = () => ingredients.Exists(x => x.CurrentState == IngredientState.Undercooked);
        overState.HasBeenReached = () => ingredients.Exists(x => x.CurrentState == IngredientState.Overcooked);
    }

    protected override void InitializeObjective()
    {
        base.InitializeObjective();
        SingletonManager.GetInstance<DialogueHintManager>().Show(dialogueHint);
        panLid.SetActive(true);
        lid.OnCoverCookware.AddListener(TurnOnVisualHint);
        stoveControllerSlider.interactable = true;
        stoveController.OnStoveSettingChanged.AddListener(SwitchCooking);

        foreach (var item in cookware.CookableIngredients)
        {
            IngredientStateController ingredient = item.IngredientInCookware.GetComponent<IngredientStateController>();
            if (ingredient)
            {
                ingredient.StartCooking(new StateSwitchOption(false, true)); // According to design, we start the cooking once again
                ingredients.Add(ingredient);
            }
        }
    }

    protected override void FinalizeObjective()
    {
        base.FinalizeObjective();
        stoveControllerSlider.interactable = false;
        stoveController.OnStoveSettingChanged.RemoveListener(SwitchCooking);
        lid.OnCoverCookware.RemoveListener(TurnOnVisualHint);
        lidAnimator.enabled = true;
        lidAnimator.SetTrigger("TakeOffDelayed");
    }

    protected override void PostFinalizeObjective()
    {
        base.PostFinalizeObjective();

        if (SuccessConditionMet())
            return;

        // Turn off the heat if it is not off
        if (stoveControllerSlider.value == 0) return;
        stoveControllerSlider.value = 0f;
        if (failHeatOffDialogue.dialogueText != string.Empty)
            SingletonManager.GetInstance<DialogueHintManager>().Show(failHeatOffDialogue);
    }

    private void SwitchCooking(HeatSetting heatSetting)
    {
        foreach (var item in ingredients)
        {
            item.IsCooking = heatSetting != HeatSetting.Off;
        }

        // NOTE: Don't automatically go to next as there's a bug that automatically registers SwipeRight for the TransitionalStep
        GoToNextObjective(false);

        // Stop the visual hint
        stoveControllerAnimator.SetBool("Blinking", false);
    }

    protected override bool SuccessConditionMet()
    {
        bool check = stoveController.CurrentHeatSetting == HeatSetting.Off
            && ingredients.Exists(x => x.CurrentState == IngredientState.Perfect);

        if (lid == null)
            return check;
        else
            return check && lid.IsCoveringCookware;
    }

    private void TurnOnVisualHint()
    {
        if (showStoveControllerHint)
            stoveControllerAnimator.SetBool("Blinking", true);

        showStoveControllerHint = false;
    }
}
