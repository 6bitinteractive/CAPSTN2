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

    [SerializeField] private DialogueHint dialogueHint;
    [SerializeField] private ObjectiveState perfectState = new ObjectiveState(ObjectiveState.Status.Perfect);
    [SerializeField] private ObjectiveState underState = new ObjectiveState(ObjectiveState.Status.Under);
    [SerializeField] private ObjectiveState overState = new ObjectiveState(ObjectiveState.Status.Over);

    private int currentAmount;

    protected override void Awake()
    {
        base.Awake();

        if (ingredientToBeAdded != null)
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

    protected override void InitializeObjective()
    {
        base.InitializeObjective();
        foodPrepUtensil.OnAddIngredient.AddListener(UpdateCount);

        if (ingredientToBeAdded != null)
            ingredientToBeAdded.gameObject.SetActive(true);

        // Show dialogue hint
        if (dialogueHint.dialogueText != string.Empty)
            SingletonManager.GetInstance<DialogueHintManager>().Show(dialogueHint);
    }

    protected override void FinalizeObjective()
    {
        base.FinalizeObjective();

        if (ingredientToBeAddedAnim != null)
        {
            if (!AnimatorUtils.IsInState(ingredientToBeAddedAnim, "AddRequiredAmountSlideOut"))
                ingredientToBeAddedAnim.SetTrigger("SlideOut");
        }

        foodPrepUtensil.OnAddIngredient.RemoveListener(UpdateCount);
    }

    protected override bool SuccessConditionMet()
    {
        return currentAmount == requiredAmount;
    }

    private void UpdateCount()
    {
        currentAmount++;
        Debug.Log("Current Added Count: " + currentAmount);

        if (currentAmount == 1)
            GoToNextObjective(false);

        if (currentAmount >= maxAmount)
        {
            if (ingredientToBeAddedAnim != null)
                ingredientToBeAddedAnim.SetTrigger("SlideOut");

            GoToNextObjective(true);
        }
    }
}
