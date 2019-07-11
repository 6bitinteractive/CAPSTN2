using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustHeatTask : Task
{
    [SerializeField] private HeatSetting requiredHeatSetting;
    public HeatSetting CurrentSetting { get; set; }

    protected override bool SuccessConditionMet()
    {
        return CurrentSetting == requiredHeatSetting;
    }
}
