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

    [SerializeField] private ObjectiveState perfectState = new ObjectiveState(ObjectiveState.Status.Perfect);
    [SerializeField] private ObjectiveState underState = new ObjectiveState(ObjectiveState.Status.Under);
    [SerializeField] private ObjectiveState overState = new ObjectiveState(ObjectiveState.Status.Over);

    private List<IngredientStateController> ingredients = new List<IngredientStateController>();
    private StoveController stoveController;

    protected override void Awake()
    {
        base.Awake();
        stoveController = stoveControllerSlider.GetComponent<StoveController>();

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
        stoveControllerSlider.interactable = true;
        stoveController.OnStoveSettingChanged.AddListener(SwicthCooking);

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
        stoveController.OnStoveSettingChanged.RemoveAllListeners();
    }

    private void SwicthCooking(HeatSetting heatSetting)
    {
        foreach (var item in ingredients)
        {
            item.IsCooking = heatSetting != HeatSetting.Off;
        }

        // NOTE: Don't automatically go to next as there's a bug that automatically registers SwipeRight for the TransitionalStep
        GoToNextObjective(false);
    }

    protected override bool SuccessConditionMet()
    {
        return stoveController.CurrentHeatSetting == HeatSetting.Off
            && ingredients.Exists(x => x.CurrentState == IngredientState.Perfect);
    }
}
