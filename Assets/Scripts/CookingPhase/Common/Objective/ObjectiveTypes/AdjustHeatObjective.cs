using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AdjustHeatObjective : Objective
{
    [SerializeField] private HeatSetting requiredHeatSetting;

    private HeatSetting currentSetting;
    private StoveController stoveController;

    protected override void InitializeObjective()
    {
        base.InitializeObjective();

        stoveController = kitchen.Equipment.GetComponentInChildren<StoveController>();
        stoveController.OnStoveSettingChanged.AddListener(SetHeatSetting);
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
        currentSetting = heatSetting;
    }
}
