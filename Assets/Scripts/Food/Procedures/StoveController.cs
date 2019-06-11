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

    public Temperature CurrentTemperature { get; private set; }

    private Slider stoveTemperatureSlider;
    private List<Slider> heatableObjects = new List<Slider>();

    private void Start()
    {
        stoveTemperatureSlider = GetComponent<Slider>();
        UpdateTemperatureText();
    }

    public void UpdateTemperatureText()
    {
        if (stoveTemperatureSlider.value == 0f)
            CurrentTemperature = Temperature.Off;
        else if (stoveTemperatureSlider.value > 0 && stoveTemperatureSlider.value <= maxLow)
            CurrentTemperature = Temperature.Low;
        else if (stoveTemperatureSlider.value <= maxMedium && stoveTemperatureSlider.value > maxLow)
            CurrentTemperature = Temperature.Medium;
        else
            CurrentTemperature = Temperature.High;
    }

    private void FixedUpdate()
    {
        if (heatableObjects.Count > 0)
        {
            foreach (var item in heatableObjects)
                ApplyHeat(item);
        }
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

        if (!heatableObjects.Contains(heatableObject)) // FIX? This will be called every loop
            heatableObjects.Add(heatableObject);
    }

    public enum Temperature
    {
        Off, Low, Medium, High
    }
}
