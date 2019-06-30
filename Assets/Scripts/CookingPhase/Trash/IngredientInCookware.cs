using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Draggable))]

public class PotIngredient : MonoBehaviour
{
    [Tooltip("The gameObject that'll be active when the player drags the ingredient to the pot.")]
    public GameObject IngredientInPot;
}
