using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Boilable : MonoBehaviour
{
    [Range(0, 1)]
    [SerializeField] private float cooling = 0.003f;
    public float MinBoil = 0.7f;
    public float MaxBoil = 0.8f;
    [SerializeField] private float timer = 10f; //?

    public bool Boiled { get; private set; }
    public Slider Temperature { get; private set; }
    public UnityEvent OnBoilingPointReached = new UnityEvent();

    private void Awake()
    {
        Temperature = GetComponentInChildren<Slider>();
    }

    private void FixedUpdate()
    {
        if (Boiled) { return; }

        if (Temperature.value <= MaxBoil && Temperature.value >= MinBoil
            && timer <= 0 && !Boiled)
        {
            OnBoilingPointReached.Invoke();
            Boiled = true;
        }

        Temperature.value -= cooling;
        timer -= Time.deltaTime;
    }
}
