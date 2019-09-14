using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddIngredientObjective : Objective
{
    [SerializeField] private Cookware cookware;
    [SerializeField] private List<GameObject> ingredientsRequired;

    protected override void InitializeObjective()
    {
        if (ingredientsRequired.Count < 1)
            Debug.LogError("No ingredients in list.");

        if (cookware == null)
            Debug.LogError("Assign which cookware to check.");
    }

    protected override bool SuccessConditionMet()
    {
        bool success = true; // We start with true in case there's only one item

        foreach (var ingredient in ingredientsRequired)
        {
            // Check each ingredient if it's in the cookware; it can only be successful if all ingredients are in the cookware
            success = success && cookware.CookableIngredients.Exists(x => x.IngredientInCookware == ingredient.gameObject);
        }

        Debug.Log("Success");
        return success;
    }
}
