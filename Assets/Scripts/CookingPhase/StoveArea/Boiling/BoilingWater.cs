using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Boilable))]

public class BoilingWater : MonoBehaviour
{
    [SerializeField] private Pot pot;

    private Boilable boilable;

    private void Start()
    {
        boilable = GetComponent<Boilable>();
        pot.AddToBoilingIngredients(boilable);
    }
}
