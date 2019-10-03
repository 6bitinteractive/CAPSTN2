using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Draggable))]

public class Cookable : MonoBehaviour
{
    [Tooltip("The gameObject representing the ingredient when it's in the cookware.")]
    public GameObject IngredientInCookware;

    //[SerializeField] private bool canCoolDown;

    //[Range(0, 1)]
    //[SerializeField] private float cooling = 0.0002f;
    //public float MinBoil = 0.7f;
    //public float MaxBoil = 0.8f;

    //public bool IsCooking { get; set; }
    //public bool PerfectlyCooked => Temperature.value <= MaxBoil && Temperature.value >= MinBoil;
    //public Slider Temperature { get; private set; }
    public UnityEvent OnIngredientDroppedToCookware = new UnityEvent();
    //public UnityEvent OnCookingStart = new UnityEvent();
    //public UnityEvent OnCookingEnd = new UnityEvent();
    //public UnityEvent OnPerfectlyCooked = new UnityEvent();

    //private void Awake()
    //{
    //    Temperature = GetComponentInChildren<Slider>();
    //    IsCooking = true;
    //}

    //private void FixedUpdate()
    //{
    //    if (Temperature == null) { return; }
    //    if (!IsCooking) { return; }

    //    if (PerfectlyCooked)
    //    {
    //        OnPerfectlyCooked.Invoke();
    //    }

    //    if (canCoolDown && Temperature.value > 0)
    //    {
    //        Temperature.value -= cooling;
    //    }
    //}

    public void ShowIngredientInCookware()
    {
        if (IngredientInCookware == null) { return; }

        // Show the ingredient
        IngredientInCookware.SetActive(true);
        OnIngredientDroppedToCookware.Invoke();

        // Hide the ingredient being dragged to the cookware
        gameObject.SetActive(false);
    }
}
