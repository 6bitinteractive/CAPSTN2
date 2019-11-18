using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// FIX: Hard-coded; for water only D:

public class MinceIngredientObjective : Objective
{
    [SerializeField] private Minceable minceableIngredient;
    [SerializeField] private ProgressBar progressBar;
    [SerializeField] private float progressIncrementValue;
    [SerializeField] private int maxRequiredTaps = 50;
    [SerializeField] private int minRequiredTaps = 37;
    [SerializeField] private DialogueHint dialogueHint;
    [SerializeField] private ObjectiveState perfectState = new ObjectiveState(ObjectiveState.Status.Perfect);
    [SerializeField] private ObjectiveState underState = new ObjectiveState(ObjectiveState.Status.Under);
    [SerializeField] private ObjectiveState overState = new ObjectiveState(ObjectiveState.Status.Over);

    private int fiftyPercent = 50;
    private int currentTaps;
    private Animator animator;

    protected override void Awake()
    {
        base.Awake();
        // Setup objectives
        // Add to list
        ObjectiveStates.Add(perfectState);
        ObjectiveStates.Add(underState);
        ObjectiveStates.Add(overState);

        //Set Percentage
        fiftyPercent = (fiftyPercent * maxRequiredTaps) / 100;
        progressBar.GetComponent<Slider>().maxValue = maxRequiredTaps;

        // Define condition
        perfectState.HasBeenReached = () => SuccessConditionMet();
        underState.HasBeenReached = () => currentTaps < minRequiredTaps;
        overState.HasBeenReached = () => currentTaps > maxRequiredTaps;

        animator = minceableIngredient.GetComponent<Animator>();
    }

    private void CheckCurrentTaps()
    {
        //UnderChopped 25% - 50%
        if (currentTaps == fiftyPercent) minceableIngredient.UpdateCurrentAnimatorController();

        //Perfect State 100%
        else if (currentTaps == minRequiredTaps) minceableIngredient.UpdateCurrentAnimatorController();

        //OverChopped
        else if (currentTaps > maxRequiredTaps) minceableIngredient.UpdateCurrentAnimatorController();
    }

    protected override void InitializeObjective()
    {
        base.InitializeObjective();
        SingletonManager.GetInstance<DialogueHintManager>().Show(dialogueHint);

        minceableIngredient.gameObject.SetActive(true);
    }

    protected override void RunObjective()
    {
        base.RunObjective();

        if (Input.GetMouseButtonDown(0))
        {
            currentTaps++;
            progressBar.IncrementProgress(progressIncrementValue);
            animator.SetTrigger("Mince");
            CheckCurrentTaps();
            minceableIngredient.Mince();

            if (currentTaps == 1)
            {
                GoToNextObjective(false); // Show next button at this point
            }
        }
    }

    protected override void FinalizeObjective()
    {
        base.FinalizeObjective();
       // minceableIngredient.gameObject.SetActive(false);
    }

    protected override bool SuccessConditionMet()
    {
        return currentTaps >= minRequiredTaps && currentTaps <= maxRequiredTaps;
    }
}
