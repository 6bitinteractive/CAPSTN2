using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Cookable))]

public class BoilingWater : MonoBehaviour
{
    [SerializeField] private Cookware pot;

    private Cookable boilable;

    private void Start()
    {
        boilable = GetComponent<Cookable>();
        pot.AddToBoilingIngredients(boilable);
    }
}
