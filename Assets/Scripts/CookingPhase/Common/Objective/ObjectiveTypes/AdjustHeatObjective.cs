﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AdjustHeatObjective : Objective
{
    [SerializeField] private HeatSetting requiredHeatSetting;

    [SerializeField] private DialogueHint failDialogue;
    [SerializeField] private ObjectiveState perfectState = new ObjectiveState(ObjectiveState.Status.Perfect);

    private HeatSetting currentSetting;
    private StoveController stoveController;
    private Slider stoveControllerSlider;
    private Animator stoveControllerAnimator;
    private bool showNextButton;

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

        stoveControllerAnimator.SetBool("Blinking", true);
        stoveControllerSlider.interactable = true;
    }

    protected override void FinalizeObjective()
    {
        base.FinalizeObjective();
        stoveControllerAnimator.SetBool("Blinking", false);
        stoveControllerSlider.interactable = false;
        stoveController.OnStoveSettingChanged.RemoveListener(SetHeatSetting);
    }

    protected override void PostFinalizeObjective()
    {
        base.PostFinalizeObjective();
        showNextButton = false;

        if (SuccessConditionMet()) { return; }

        switch (requiredHeatSetting)
        {
            case HeatSetting.Off:
                stoveControllerSlider.value = 0f;
                break;
            case HeatSetting.Low:
                stoveControllerSlider.value = StoveController.lowSettingValue;
                break;
            case HeatSetting.Medium:
                stoveControllerSlider.value = StoveController.mediumSettingValue;
                break;
            case HeatSetting.High:
                stoveControllerSlider.value = StoveController.highSettingValue;
                break;
        }

        if (failDialogue.dialogueText != string.Empty)
            SingletonManager.GetInstance<DialogueHintManager>().Show(failDialogue);
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
