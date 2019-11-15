using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreheatCookwareAddIngredientObjective : Objective
{
    [SerializeField] private List<Cookable> ingredientsToBeAdded;
    [SerializeField] private GameObject steam;
    [SerializeField] private float steamWaitTime = 5f;
    [SerializeField] private DialogueHint dialogueHint;

    private bool preheated, addedBeforePreheated;
    private int currentIngredient;

    [SerializeField] private ObjectiveState perfectState = new ObjectiveState(ObjectiveState.Status.Perfect);

    protected override void Awake()
    {
        base.Awake();

        // Setup objectives
        // Add to list
        ObjectiveStates.Add(perfectState);

        // Define condition
        perfectState.HasBeenReached = () => preheated && !addedBeforePreheated;
    }

    protected override void InitializeObjective()
    {
        base.InitializeObjective();
        SingletonManager.GetInstance<DialogueHintManager>().Show(dialogueHint);

        if (ingredientsToBeAdded.Count == 0)
            Debug.LogError("Specify which ingredients are to be added.");

        // Listen to event if previous ingredient has been dropped
        foreach (var ingredient in ingredientsToBeAdded)
        {
            ingredient.OnIngredientDroppedToCookware.AddListener(AddNextIngredient);
        }

        // Enable first ingredient
        ingredientsToBeAdded[currentIngredient].gameObject.SetActive(true);

        Invoke("LetOutSteam", steamWaitTime);
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

        return success && !addedBeforePreheated;
    }

    private void AddNextIngredient()
    {
        currentIngredient++;

        // Check if player added an ingredient before steam came out
        if (!preheated)
            addedBeforePreheated = true;


        if (currentIngredient >= ingredientsToBeAdded.Count)
        {
            GoToNextObjective(true);
            return;
        }

        ingredientsToBeAdded[currentIngredient].gameObject.SetActive(true);
    }

    private void LetOutSteam()
    {
        steam.SetActive(true);
        preheated = true;
    }
}
