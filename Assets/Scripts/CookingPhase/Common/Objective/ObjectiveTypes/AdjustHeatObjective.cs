using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AdjustHeatObjective : Objective
{
    [SerializeField] private HeatSetting requiredHeatSetting;

    private HeatSetting currentSetting;
    private StoveController stoveController;
    private Animator stoveControllerAnimator;

    protected override void InitializeObjective()
    {
        base.InitializeObjective();

        stoveController = kitchen.Equipment.GetComponentInChildren<StoveController>();
        stoveControllerAnimator = stoveController.GetComponent<Animator>();
        stoveController.OnStoveSettingChanged.AddListener(SetHeatSetting);

        stoveControllerAnimator.SetBool("Blinking", true);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        stoveController.OnStoveSettingChanged.RemoveAllListeners();
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
