using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoveControllerUI : MonoBehaviour
{
    [Range(0, 1)]
    [SerializeField] private float maxLow = 0.3f, maxMedium = 0.7f;

    [Range(0, 1)]
    public float AddLowHeat = 0.0003f, AddMediumHeat = 0.0006f, AddHighHeat = 0.001f;

    public TemperatureSetting CurrentTemperatureSetting { get; private set; }

    private Slider stoveTemperatureSlider;

    private void Start()
    {
        stoveTemperatureSlider = GetComponentInChildren<Slider>();
    }

    public void UpdateTemperatureSetting()
    {
        if (stoveTemperatureSlider.value == 0f)
            CurrentTemperatureSetting = TemperatureSetting.Off;
        else if (stoveTemperatureSlider.value > 0 && stoveTemperatureSlider.value <= maxLow)
            CurrentTemperatureSetting = TemperatureSetting.Low;
        else if (stoveTemperatureSlider.value <= maxMedium && stoveTemperatureSlider.value > maxLow)
            CurrentTemperatureSetting = TemperatureSetting.Medium;
        else
            CurrentTemperatureSetting = TemperatureSetting.High;
    }
}

public enum TemperatureSetting
{
    Off, Low, Medium, High
}
