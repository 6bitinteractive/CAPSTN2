using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoveController : MonoBehaviour
{
    [Range(0, 1)]
    [SerializeField] private float maxLow, maxMedium;
    [SerializeField] private float addLowHeat = 0.05f, addMediumHeat = 0.07f, addHighHeat = 0.1f;

    [SerializeField] private TextMeshProUGUI temperatureText;
    private Slider slider;

    public Temperature CurrentTemperature { get; private set; }

    private void Start()
    {
        slider = GetComponent<Slider>();
        UpdateTemperatureText();
    }

    public void UpdateTemperatureText()
    {
        if (slider.value == 0f)
            CurrentTemperature = Temperature.Off;
        else if (slider.value > 0 && slider.value <= maxLow)
            CurrentTemperature = Temperature.Low;
        else if (slider.value <= maxMedium && slider.value > maxLow)
            CurrentTemperature = Temperature.Medium;
        else
            CurrentTemperature = Temperature.High;

        temperatureText.text = CurrentTemperature.ToString();
    }

    public void ApplyHeat(Slider heatableObject)
    {
        // TODO: Apply heat over time
        float heat = 0;
        switch (CurrentTemperature)
        {
            case Temperature.Low:
                heat = addLowHeat;
                break;
            case Temperature.Medium:
                heat = addMediumHeat;
                break;
            case Temperature.High:
                heat = addHighHeat;
                break;
            default:
                heat = 0;
                break;
        }

        heatableObject.value += heat;
    }

    public enum Temperature
    {
        Off, Low, Medium, High
    }
}
