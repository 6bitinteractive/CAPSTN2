using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    private Slider slider;
    private float currentProgress;

    void Start()
    {
        slider = gameObject.GetComponent<Slider>();
    }

    public void IncrementProgress(float newValue)
    {
        slider.value += newValue;
    }
}