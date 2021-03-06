﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddIngredientObjective : Objective
{
    [SerializeField] private List<Cookable> ingredientsToBeAdded;
    [SerializeField] private DialogueHint dialogueHint;
    [SerializeField] private ObjectiveState perfectState = new ObjectiveState(ObjectiveState.Status.Perfect);

    private int currentIngredient;

    protected override void Awake()
    {
        base.Awake();

        // Setup objectives
        // Add to list
        ObjectiveStates.Add(perfectState);

        // Define condition
        perfectState.HasBeenReached = () => SuccessConditionMet();
    }

    protected override void InitializeObjective()
    {
        base.InitializeObjective();

        if (ingredientsToBeAdded.Count == 0)
            Debug.LogError("Specify which ingredients are to be added.");

        // Listen to event if previous ingredient has been dropped
        foreach (var ingredient in ingredientsToBeAdded)
        {
            ingredient.OnIngredientDroppedToCookware.AddListener(AddNextIngredient);
        }

        // Show dialogue hint
        if (dialogueHint.dialogueText != string.Empty)
            SingletonManager.GetInstance<DialogueHintManager>().Show(dialogueHint);

        // Enable first ingredient
        ingredientsToBeAdded[currentIngredient].gameObject.SetActive(true);
    }

    protected override void FinalizeObjective()
    {
        base.FinalizeObjective();

        foreach (var ingredient in ingredientsToBeAdded)
        {
            ingredient.OnIngredientDroppedToCookware.RemoveListener(AddNextIngredient);
            ingredient.gameObject.SetActive(false);
        }
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

        if (currentIngredient >= ingredientsToBeAdded.Count)
        {
            GoToNextObjective(true);
            return;
        }

        ingredientsToBeAdded[currentIngredient].gameObject.SetActive(true);
    }
}
