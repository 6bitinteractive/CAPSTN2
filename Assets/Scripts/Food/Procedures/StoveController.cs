using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoveController : MonoBehaviour
{
    [Range(0, 1)]
    [SerializeField] private float maxLow, maxMedium;

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

    public enum Temperature
    {
        Off, Low, Medium, High
    }
}
