using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeObjective : Objective
{
    [Tooltip("Make sure it's not the ShakerPanel's animator.")]
    [SerializeField] private Animator shaker;
    [SerializeField] private int requiredAmount = 1;
    [SerializeField] private int maxAmount = 5;

    [SerializeField] private ObjectiveState perfectState = new ObjectiveState(ObjectiveState.Status.Perfect);
    [SerializeField] private ObjectiveState underState = new ObjectiveState(ObjectiveState.Status.Under);
    [SerializeField] private ObjectiveState overState = new ObjectiveState(ObjectiveState.Status.Over);

    private int count;
    private bool canUseShaker = true;

    protected override void Awake()
    {
        base.Awake();

        // Setup objectives
        // Add to list
        ObjectiveStates.Add(perfectState);
        ObjectiveStates.Add(underState);
        ObjectiveStates.Add(overState);

        // Define condition
        perfectState.HasBeenReached = () => SuccessConditionMet();
        underState.HasBeenReached = () => count < requiredAmount && count != 0;
        overState.HasBeenReached = () => count > requiredAmount;
    }

    protected override void InitializeObjective()
    {
        base.InitializeObjective();

        shaker.transform.parent.gameObject.SetActive(true);
        GoToNextObjective(false);
    }

    protected override void RunObjective()
    {
        base.RunObjective();

        if (UsedShaker())
        {
            canUseShaker = false;
            StartCoroutine(ShakeContainer());
        }
    }

    private IEnumerator ShakeContainer()
    {
        // If maxAmount is reached
        if (count >= maxAmount)
        {
            GoToNextObjective(true);
            yield break;
        }

        // Play animation
        shaker.SetTrigger("Shake");
        yield return new WaitWhile(() => AnimatorUtils.IsInState(shaker, "Shake") && AnimatorUtils.IsDonePlaying(shaker, "Shake"));

        // Increase count
        count++;
        Debug.Log("Used shaker: " + count + "x.");

        // Allow to use shaker again
        canUseShaker = true;
    }

    protected override void FinalizeObjective()
    {
        base.FinalizeObjective();

        shaker.transform.parent.gameObject.GetComponent<Animator>().SetTrigger("SlideOut");
    }

    private bool UsedShaker()
    {
        #region Standalone Input
#if UNITY_STANDALONE_WIN
        // Test
        return Input.GetMouseButtonDown(1);
#endif
        #endregion

        #region Mobile Input
#if UNITY_ANDROID || UNITY_IOS

        return false;

#endif
        #endregion
    }

    protected override bool SuccessConditionMet()
    {
        return count == requiredAmount;
    }
}
