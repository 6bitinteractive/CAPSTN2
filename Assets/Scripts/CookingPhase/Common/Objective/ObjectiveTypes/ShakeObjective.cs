using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class ShakeObjective : Objective
{
    [Tooltip("Make sure it's not the ShakerPanel's animator.")]
    [SerializeField] private Animator shaker;
    [SerializeField] private int requiredAmount = 1;
    [SerializeField] private int maxAmount = 5;

    [SerializeField] private ObjectiveState perfectState = new ObjectiveState(ObjectiveState.Status.Perfect);
    [SerializeField] private ObjectiveState underState = new ObjectiveState(ObjectiveState.Status.Under);
    [SerializeField] private ObjectiveState overState = new ObjectiveState(ObjectiveState.Status.Over);

    private AudioSource audioSource;
    private int count;
    private bool canUseShaker = true;
    private Vector3 Tilt;
    private bool isPullingRight;

    protected override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();

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

        if (canUseShaker && UsedShaker())
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
        yield return new WaitUntil(() => AnimatorUtils.IsInState(shaker, "Shake") && AnimatorUtils.IsDonePlaying(shaker, "Shake"));
        shaker.SetTrigger("Reset");

        // Play sound
        audioSource.Play();

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
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
        // Test
        //return Input.GetMouseButtonDown(1);
        return Input.GetKeyDown(KeyCode.A);
#endif
        #endregion

        #region Mobile Input
#if UNITY_ANDROID || UNITY_IOS

          return CheckShake();
#endif
        #endregion
    }

    protected override bool SuccessConditionMet()
    {
        return count == requiredAmount;
    }

    private bool CheckShake()
    {
        Tilt = Input.acceleration;

        // Debug.Log("Current Tilt: " + Tilt);

        // Pulling phone rightwards
        if (Tilt.x > 0.4f)
        {
            isPullingRight = true;
        }

        // Moving phone leftwards when sucessfully pulled rightwards first
        if (isPullingRight && Tilt.x < -0.4f)
        {
            isPullingRight = false;
            return true;
        }
        return false;
    }
}
