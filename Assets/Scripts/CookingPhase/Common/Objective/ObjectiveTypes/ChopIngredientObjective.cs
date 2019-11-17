using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChopIngredientObjective : Objective
{
    [SerializeField] private Choppable ingredientToSlice;
    [SerializeField] private bool tap;
    [SerializeField] private ObjectiveState perfectState = new ObjectiveState(ObjectiveState.Status.Perfect);

    public bool Tap { get => tap; set => tap = value; }

    protected override void Awake()
    {
        base.Awake();

        // Setup objectives
        // Add to list
        ObjectiveStates.Add(perfectState);

        // Define condition
        perfectState.HasBeenReached = () => SuccessConditionMet();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void InitializeObjective()
    {
        base.InitializeObjective();
        perfectState.OnStateReached.AddListener((x) => SliceEnd());
        ingredientToSlice.gameObject.SetActive(true);
    }

    protected override void FinalizeObjective()
    {
        base.FinalizeObjective();
        StopAllCoroutines();
        ingredientToSlice.gameObject.SetActive(false);
    }

    protected override void RunObjective()
    {
        base.RunObjective();

        // Check if tap type
        if (!Tap) { return; }

        if (Input.GetMouseButtonDown(0))
        {
            SliceIngredient();
        }
    }

    public void SliceIngredient()
    {
        ingredientToSlice.OnChop();
    }

    public void SliceEnd()
    {
        GoToNextObjective(false);
        ingredientToSlice.OnChopEnd();
    }

    protected override bool SuccessConditionMet()
    {
        return ingredientToSlice.MaxSliceReached();
    }
}
