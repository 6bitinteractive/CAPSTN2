using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StirFryObjective : Objective
{
    [SerializeField] private SpoonKitchenUtensil spoon;
    [SerializeField] private float minDuration = 7f;
    [SerializeField] private float maxDuration = 15f;

    [SerializeField] private DialogueHint dialogueHint;
    [SerializeField] private ObjectiveState perfectState = new ObjectiveState(ObjectiveState.Status.Perfect);
    [SerializeField] private ObjectiveState underState = new ObjectiveState(ObjectiveState.Status.Under);
    [SerializeField] private ObjectiveState overState = new ObjectiveState(ObjectiveState.Status.Over);

    private Animator spoonAnim;
    private bool showNextButton = true;

    protected override void Awake()
    {
        base.Awake();
        spoonAnim = spoon.GetComponent<Animator>();

        // Setup objectives
        // Add to list
        ObjectiveStates.Add(perfectState);
        ObjectiveStates.Add(underState);
        ObjectiveStates.Add(overState);

        // Define condition
        perfectState.HasBeenReached = () => SuccessConditionMet();
        underState.HasBeenReached = () => spoon.MixDuration < minDuration && spoon.MixDuration > 2f;
        overState.HasBeenReached = () => spoon.MixDuration > maxDuration;
    }

    protected override void InitializeObjective()
    {
        base.InitializeObjective();
        SingletonManager.GetInstance<DialogueHintManager>().Show(dialogueHint);

        if (spoon.gameObject.activeInHierarchy)
            spoonAnim.SetTrigger("SlideIn");
        else
            spoon.gameObject.SetActive(true);
    }

    protected override void RunObjective()
    {
        base.RunObjective();

        // If player has mixed for a few seconds, show the Next button
        if (spoon.MixDuration >= 3f && showNextButton)
        {
            showNextButton = false;
            GoToNextObjective(false);
        }

        Debug.Log(spoon.MixDuration);
    }

    protected override void FinalizeObjective()
    {
        base.FinalizeObjective();
        spoon.MixDuration = 0f;
        spoonAnim.SetTrigger("SlideOut");
    }

    protected override bool SuccessConditionMet()
    {
        return spoon.MixDuration >= minDuration && spoon.MixDuration <= maxDuration;
    }
}
