using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]

[System.Serializable]
public class OnStoveSettingChanged : UnityEvent<HeatSetting> { }

public class StoveController : MonoBehaviour
{
    [Range(0, 1)]
    [SerializeField] private float maxLow = 0.3f, maxMedium = 0.7f;

    public HeatSetting CurrentHeatSetting { get; private set; }
    public OnStoveSettingChanged OnStoveSettingChanged = new OnStoveSettingChanged();

    public void UpdateStoveSetting(float sliderValue)
    {
        if (sliderValue == 0f)
            //CurrentHeatSetting = HeatSetting.Off;
            CurrentHeatSetting = HeatSetting.Low; // NOTE: Current art implementation has no "off" setting
        else if (sliderValue > 0 && sliderValue <= maxLow)
            CurrentHeatSetting = HeatSetting.Low;
        else if (sliderValue <= maxMedium && sliderValue > maxLow)
            CurrentHeatSetting = HeatSetting.Medium;
        else
            CurrentHeatSetting = HeatSetting.High;

        OnStoveSettingChanged.Invoke(CurrentHeatSetting);
    }
}

public enum HeatSetting
{
    Off, Low, Medium, High
}
