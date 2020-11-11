using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class ShakeObjective2 : Objective
{
    [Tooltip("Make sure it's not the ShakerPanel's animator.")]
    [SerializeField] private Animator shaker;
    [SerializeField] private ParticleSystem psShaker;
    [SerializeField] [Range(0,1)] private float minPercentNeeded = .3f;
    [SerializeField] [Range(0, 1)] private float maxPercentNeeded = .5f;
    [SerializeField] private float speed = 1.0f;

    [SerializeField] private ObjectiveState perfectState = new ObjectiveState(ObjectiveState.Status.Perfect);
    [SerializeField] private ObjectiveState underState = new ObjectiveState(ObjectiveState.Status.Under);
    [SerializeField] private ObjectiveState overState = new ObjectiveState(ObjectiveState.Status.Over);

    private AudioSource audioSource;
    private int count;
    private bool canUseShaker = true;
    private float meterPercentage = 0;
    private float elapsedTime = 0;
    private float endPercentage = 0;
    private int burstAmount = 0;

    public float CurrentMeterPercentage {  get { return meterPercentage; } }

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
        underState.HasBeenReached = () => endPercentage < minPercentNeeded && endPercentage != 0;
        overState.HasBeenReached = () => endPercentage > maxPercentNeeded;
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
            endPercentage = meterPercentage;
            canUseShaker = false;
            StartCoroutine(ShakeContainer());
        }

        if (canUseShaker)
        {
            elapsedTime += Time.deltaTime;
            meterPercentage = (1.0f + Mathf.Sin((elapsedTime - 180) * Mathf.Deg2Rad * speed)) / 2f;
        }
    }

    private IEnumerator ShakeContainer()
    {
        GoToNextObjective(true);

        shaker.GetComponent<InPosition>().OnDestinationReached.AddListener(()=>psShaker.Emit(burstAmount));

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
        return Input.GetKeyDown(KeyCode.Space);
        //return InputGet
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
        return endPercentage > minPercentNeeded && endPercentage < maxPercentNeeded;
    }

    public void SetBurst(int amount)
    {
        burstAmount = amount;
    }
}
