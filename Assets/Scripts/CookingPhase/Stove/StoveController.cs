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
    // Default Values
    public static float lowSettingValue = 0.18f, mediumSettingValue = 0.674f, highSettingValue = 0.985f;

    [Range(0, 1)]
    [SerializeField] private float maxLow = 0.3f, maxMedium = 0.7f;

    public HeatSetting CurrentHeatSetting { get; private set; }
    public OnStoveSettingChanged OnStoveSettingChanged = new OnStoveSettingChanged();

    private HeatSetting previousSetting;

    public void UpdateStoveSetting(float sliderValue)
    {
        previousSetting = CurrentHeatSetting;

        if (sliderValue <= 0f)
            CurrentHeatSetting = HeatSetting.Off;
        //CurrentHeatSetting = HeatSetting.Low; // NOTE: Current art implementation has no "off" setting
        else if (sliderValue > 0 && sliderValue <= maxLow)
            CurrentHeatSetting = HeatSetting.Low;
        else if (sliderValue <= maxMedium && sliderValue > maxLow)
            CurrentHeatSetting = HeatSetting.Medium;
        else
            CurrentHeatSetting = HeatSetting.High;

        if (CurrentHeatSetting != previousSetting)
            OnStoveSettingChanged.Invoke(CurrentHeatSetting);
    }
}

public enum HeatSetting
{
    Off, Low, Medium, High
}
