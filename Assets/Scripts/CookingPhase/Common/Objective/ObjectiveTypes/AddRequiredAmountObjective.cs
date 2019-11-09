using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* NOTE
 * Sometimes, the amount added is doubled so the value of the currentAmount being checked will be wrong.
 * Save then unload the scene then try to run the scene again.
 */

public class AddRequiredAmountObjective : Objective
{
    [SerializeField] private int requiredAmount = 5;
    [SerializeField] private int minAmount = 1;
    [SerializeField] private int maxAmount = 8;

    [SerializeField] private FoodPrepUtensil foodPrepUtensil;
    [SerializeField] private GameObject ingredientToBeAdded;
    private Animator ingredientToBeAddedAnim;

    [SerializeField] private ObjectiveState perfectState = new ObjectiveState(ObjectiveState.Status.Perfect);
    [SerializeField] private ObjectiveState underState = new ObjectiveState(ObjectiveState.Status.Under);
    [SerializeField] private ObjectiveState overState = new ObjectiveState(ObjectiveState.Status.Over);

    private int currentAmount;

    protected override void Awake()
    {
        base.Awake();

        ingredientToBeAddedAnim = ingredientToBeAdded.GetComponent<Animator>();

        // Setup objectives
        // Add to list
        ObjectiveStates.Add(perfectState);
        ObjectiveStates.Add(underState);
        ObjectiveStates.Add(overState);

        // Define condition
        perfectState.HasBeenReached = () => SuccessConditionMet();
        underState.HasBeenReached = () => currentAmount == 1;
        overState.HasBeenReached = () => currentAmount > requiredAmount;
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
        //Debug.Log("Current count: " + currentAmount);

        if (currentAmount == 1)
            GoToNextObjective(false);

        if (currentAmount >= maxAmount)
            ingredientToBeAddedAnim.SetTrigger("SlideOut");
    }
}
