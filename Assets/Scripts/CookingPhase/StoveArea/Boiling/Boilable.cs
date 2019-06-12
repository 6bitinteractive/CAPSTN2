using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Boilable : MonoBehaviour
{
    [SerializeField] private bool canCoolDown;

    [Range(0, 1)]
    [SerializeField] private float cooling = 0.0002f;
    public float MinBoil = 0.7f;
    public float MaxBoil = 0.8f;

    public bool IsBoiling { get; set; }
    public Slider Temperature { get; private set; }
    public bool BoiledAtRightTemperature => Temperature.value <= MaxBoil && Temperature.value >= MinBoil;
    public UnityEvent OnBoilingStart = new UnityEvent();
    public UnityEvent OnBoilingEnd = new UnityEvent();


    public UnityEvent OnBoilingPointReached = new UnityEvent();

    private void Awake()
    {
        Temperature = GetComponentInChildren<Slider>();
        IsBoiling = true;
    }

    private void FixedUpdate()
    {
        if (!IsBoiling) { return; }

        if (BoiledAtRightTemperature)
        {
            OnBoilingPointReached.Invoke();
        }

        if (canCoolDown && Temperature.value > 0)
        {
            Temperature.value -= cooling;
        }
    }
}
