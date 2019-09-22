using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForScumObjective : Objective
{
    [SerializeField] private WaterScum waterScum;
    [SerializeField] private DialogueHint dialogueHint;

    private Coroutine coroutine;

    [SerializeField] private ObjectiveState perfectState = new ObjectiveState(ObjectiveState.Status.Perfect);
    [SerializeField] private ObjectiveState underState = new ObjectiveState(ObjectiveState.Status.Under);

    protected override void Awake()
    {
        base.Awake();

        // Setup objectives
        // Add to list
        ObjectiveStates.Add(perfectState);
        ObjectiveStates.Add(underState);

        // Define condition
        perfectState.HasBeenReached = () => waterScum.currentIndex == waterScum.waterScum.Count; // Player has seen all the scum that can form
        underState.HasBeenReached = () => waterScum.currentIndex == 2; // Show hint only after the first layer of scum has formed
    }

    protected override void InitializeObjective()
    {
        base.InitializeObjective();

        // Make sure required objects are active
        waterScum.transform.parent.gameObject.SetActive(true);
        waterScum.gameObject.SetActive(true);

        coroutine = StartCoroutine(waterScum.Display());
        SingletonManager.GetInstance<DialogueHintManager>().Show(dialogueHint);
    }

    protected override void FinalizeObjective()
    {
        base.FinalizeObjective();
        StopCoroutine(coroutine);
    }

    protected override bool SuccessConditionMet()
    {
        return waterScum.Displayed;
    }
}
