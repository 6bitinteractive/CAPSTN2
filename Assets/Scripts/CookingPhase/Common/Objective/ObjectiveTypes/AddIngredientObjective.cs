using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddIngredientObjective : Objective
{
    [SerializeField] private List<Cookable> ingredientsToBeAdded;
    private int currentIngredient;

    protected override void OnEnable()
    {
        base.OnEnable();

        // Listen to event if previous ingredient has been dropped
        foreach (var ingredient in ingredientsToBeAdded)
        {
            ingredient.OnIngredientDroppedToCookware.AddListener(AddNextIngredient);
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        foreach (var ingredient in ingredientsToBeAdded)
        {
            ingredient.OnIngredientDroppedToCookware.RemoveAllListeners();
        }
    }

    protected override void InitializeObjective()
    {
        if (ingredientsToBeAdded.Count == 0)
            Debug.LogError("Specify which ingredients are to be added.");

        // Enable first ingredient
        ingredientsToBeAdded[currentIngredient].gameObject.SetActive(true);
    }

    protected override bool SuccessConditionMet()
    {
        bool success = true; // We start with true in case there's only one item

        foreach (var ingredient in ingredientsToBeAdded)
        {
            // Check each ingredient if it's in the cookware; it can only be successful if *all* ingredients are in the cookware
            success = success && kitchen.Cookware.CookableIngredients.Exists(x => x.IngredientInCookware == ingredient.IngredientInCookware);
        }

        return success;
    }

    private void AddNextIngredient()
    {
        currentIngredient++;
        if (currentIngredient >= ingredientsToBeAdded.Count) { return; }

        ingredientsToBeAdded[currentIngredient].gameObject.SetActive(true);
    }
}
