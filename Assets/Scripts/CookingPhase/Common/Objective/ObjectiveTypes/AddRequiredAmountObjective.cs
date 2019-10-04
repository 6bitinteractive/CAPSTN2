using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRequiredAmountObjective : Objective
{
    [SerializeField] private int requiredAmount = 5;
    [SerializeField] private int minAmount = 1;
    [SerializeField] private int maxAmount = 8;

    [SerializeField] private FoodPrepUtensil foodPrepUtensil;
    [SerializeField] private GameObject ingredientToBeAdded;
    private Animator ingredientToBeAddedAnim;

    [SerializeField] private ObjectiveState perfectState = new ObjectiveState(ObjectiveState.Status.Perfect);

    private int currentAmount;

    protected override void Awake()
    {
        base.Awake();

        ingredientToBeAddedAnim = ingredientToBeAdded.GetComponent<Animator>();

        // Setup objectives
        // Add to list
        ObjectiveStates.Add(perfectState);

        // Define condition
        perfectState.HasBeenReached = () => SuccessConditionMet();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        foodPrepUtensil.OnAddIngredient.AddListener(UpdateCount);
    }

    protected override void InitializeObjective()
    {
        base.InitializeObjective();
        ingredientToBeAdded.gameObject.SetActive(true);
        GoToNextObjective(false);
    }

    protected override void FinalizeObjective()
    {
        base.FinalizeObjective();

        if (!AnimatorUtils.IsInState(ingredientToBeAddedAnim, "AddRequiredAmountSlideOut"))
            ingredientToBeAddedAnim.SetTrigger("SlideOut");
    }

    protected override bool SuccessConditionMet()
    {
        return currentAmount == requiredAmount;
    }

    private void UpdateCount()
    {
        currentAmount++;

        if (currentAmount >= maxAmount)
            ingredientToBeAddedAnim.SetTrigger("SlideOut");
    }
}
