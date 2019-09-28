using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChopIngredientObjective : Objective
{
    [SerializeField] private int maxIngredientSlices = 0;
    [SerializeField] private List<Choppable> ingredientsToSlice;

    private Queue<Choppable> ingredientsLeft;
    private Choppable currentIngredient;
    private int currentIngredientCount;
    private int sliceCount = 0;

    [SerializeField] private ObjectiveState perfectState = new ObjectiveState(ObjectiveState.Status.Perfect);

    protected override void Awake()
    {
        base.Awake();

        // Setup objectives
        // Add to list
        ObjectiveStates.Add(perfectState);

        // Define condition
        perfectState.HasBeenReached = () => SuccessConditionMet();

        // Set the max ingredient slices
        for (int i = 0; i < ingredientsToSlice.Count; i++)
        {
           maxIngredientSlices += ingredientsToSlice[i].MaxIngredientSlices;
        }
    }
  
    protected override void OnEnable()
    {
        base.OnEnable();

        // Listen to event if previous ingredient has been dropped
        foreach (var ingredient in ingredientsToSlice)
        {
            ingredient.OnIngredientChopEnd.AddListener(AddNextIngredient);
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        foreach (var ingredient in ingredientsToSlice)
        {
            ingredient.OnIngredientChopEnd.RemoveAllListeners();
        }
    }
    
    protected override void InitializeObjective()
    {
        if (ingredientsToSlice.Count == 0)
            Debug.LogError("Specify which ingredients are to be added.");

        // Enable first ingredient
        ingredientsLeft = new Queue<Choppable>(ingredientsToSlice);
        ingredientsToSlice[currentIngredientCount].gameObject.SetActive(true);
        BeginSlicingIngredients();
    }

    protected override void FinalizeObjective()
    {
        base.FinalizeObjective();
        StopAllCoroutines();

        foreach (var ingredient in ingredientsToSlice)
        {
            ingredient.gameObject.SetActive(false);
        }
    }

    public void BeginSlicingIngredients()
    {
        currentIngredient = ingredientsLeft.Peek();
        currentIngredient.gameObject.SetActive(true);
        ingredientsLeft.Dequeue();
    }

    public void SliceIngredient()
    {
        sliceCount++;
        currentIngredient.OnChop();
    }

    public bool MaxSliceReached()
    {
        return sliceCount >= maxIngredientSlices;
    }

    private bool StageEnd()
    {
        return ingredientsLeft.Count == 0;
    }

    protected override bool SuccessConditionMet()
    {
        bool success = true; // We start with true in case there's only one item

        foreach (var ingredient in ingredientsToSlice)
        {
           success = success && ingredient.IsChopped == true;
        }

        return success;
    }

    private void AddNextIngredient()
    {
        currentIngredientCount++;

        if (StageEnd())
        {
            GoToNextObjective(true);
            return;
        }

        BeginSlicingIngredients();
        ingredientsToSlice[currentIngredientCount].gameObject.SetActive(true);
    }
}
