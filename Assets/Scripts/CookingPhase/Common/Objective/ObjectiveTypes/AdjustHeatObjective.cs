using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AdjustHeatObjective : Objective
{
    [SerializeField] private HeatSetting requiredHeatSetting;

    private HeatSetting currentSetting;
    private StoveController stoveController;
    private Slider stoveControllerSlider;
    private Animator stoveControllerAnimator;

    private bool showNextButton;
    [SerializeField] private ObjectiveState perfectState = new ObjectiveState(ObjectiveState.Status.Perfect);

    // FIX: Null reference when objective is done?
    //protected override void OnDestroy()
    //{
    //    base.OnDestroy();

    //    stoveController.OnStoveSettingChanged.RemoveAllListeners();
    //}

    protected override void Awake()
    {
        base.Awake();

        // Setup objectives
        // Add to list
        ObjectiveStates.Add(perfectState);

        // Define condition
        perfectState.HasBeenReached = () => SuccessConditionMet();
    }

    protected override void InitializeObjective()
    {
        base.InitializeObjective();

        stoveController = kitchen.Equipment.GetComponentInChildren<StoveController>();
        stoveControllerSlider = stoveController.GetComponent<Slider>();
        stoveControllerAnimator = stoveController.GetComponent<Animator>();
        stoveController.OnStoveSettingChanged.AddListener(SetHeatSetting);

        // Hack... for cases when the player already has the correct setting (most likely because they incorrectly set the heat prior to this objective)
        // We manually invoke the event so that it can be checked right away
        stoveController.OnStoveSettingChanged.Invoke(stoveController.CurrentHeatSetting);

        stoveControllerAnimator.SetBool("Blinking", true);
        stoveControllerSlider.interactable = true;
    }

    protected override void FinalizeObjective()
    {
        base.FinalizeObjective();
        stoveControllerAnimator.SetBool("Blinking", false);
        stoveControllerSlider.interactable = false;
        showNextButton = false;
        stoveController.OnStoveSettingChanged.RemoveListener(SetHeatSetting);
    }

    protected override bool SuccessConditionMet()
    {
        //Debug.Log("Current Heat Setting: " + currentSetting);
        return currentSetting == requiredHeatSetting;
    }

    private void SetHeatSetting(HeatSetting heatSetting)
    {
        if (!showNextButton)
        {
            showNextButton = true;
            GoToNextObjective(false);
        }

        stoveControllerAnimator.SetBool("Blinking", false); // FIX: Avoid calling multiple times
        currentSetting = heatSetting;
    }
}
