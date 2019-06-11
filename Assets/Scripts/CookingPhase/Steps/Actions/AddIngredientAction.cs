using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddIngredientAction : Action
{
    [SerializeField] private List<GameObject> ingredients;

    public override void ResetAction()
    {
        foreach (var ingredient in ingredients)
        {
            ingredient.SetActive(false);
        }
    }

    public override void Begin()
    {
        foreach (var ingredient in ingredients)
        {
            ingredient.SetActive(true);
        }

        Active = true;
    }
}
