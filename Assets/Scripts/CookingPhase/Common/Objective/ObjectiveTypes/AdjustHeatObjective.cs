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

    // FIX: Null reference when objective is done?
    //protected override void OnDestroy()
    //{
    //    base.OnDestroy();

    //    stoveController.OnStoveSettingChanged.RemoveAllListeners();
    //}

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
    }

    protected override bool SuccessConditionMet()
    {
        Debug.Log("Current Heat Setting: " + currentSetting);
        return currentSetting == requiredHeatSetting;
    }

    private void SetHeatSetting(HeatSetting heatSetting)
    {
        stoveControllerAnimator.SetBool("Blinking", false); // FIX: Avoid calling multiple times
        currentSetting = heatSetting;
    }
}
