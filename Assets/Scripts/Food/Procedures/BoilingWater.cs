using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]

public class BoilingWater : MonoBehaviour
{
    [Range(0, 1)]
    [SerializeField] private float cooling = 0.003f;
    [SerializeField] private float minBoil = 0.7f;
    [SerializeField] private float maxBoil = 0.8f;

    public bool Boiled { get; private set; }

    public UnityEvent OnBoilingPointReached = new UnityEvent();

    private Slider slider;
    private float timer = 10f;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    private void FixedUpdate()
    {
        if (Boiled) { return; }

        if (slider.value <= maxBoil && slider.value >= minBoil
            && timer <= 0 && !Boiled)
        {
            OnBoilingPointReached.Invoke();
            Boiled = true;
        }

        slider.value -= cooling;
        timer -= Time.deltaTime;
    }
}
