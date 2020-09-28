using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CircleGestureDetector))]

public class StirObjective : Objective
{
    [SerializeField] private int requiredStirCount = 5;
    [SerializeField] private ProgressMeter progressMeter;
    [SerializeField] private Image waterThickenedImage;
    [SerializeField] private KitchenUtensil spoon;
    [SerializeField] private ObjectiveState perfectState = new ObjectiveState(ObjectiveState.Status.Perfect);

    private CircleGestureDetector circleGestureDetector;
    private AudioSource audioSource;
    private float stirCountLimit;
    //private float minPerfectCount, maxPerfectCount;
    private int currentCount;

    protected override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
        circleGestureDetector = GetComponent<CircleGestureDetector>();
        circleGestureDetector.enabled = false;

        if (waterThickenedImage == null)
            Debug.LogWarning("Image not set");

        // Setup objectives
        // Add to list
        ObjectiveStates.Add(perfectState);

        // Define condition
        perfectState.HasBeenReached = () => SuccessConditionMet();

        // Math
        stirCountLimit = requiredStirCount / progressMeter.PerfectMid;
        Debug.Log("Stir Count Limit: " + stirCountLimit);
        //minPerfectCount = stirCountLimit * progressMeter.perfectMin;
        //maxPerfectCount = stirCountLimit * progressMeter.perfectMax;
    }

    protected override void InitializeObjective()
    {
        base.InitializeObjective();
        circleGestureDetector.enabled = true;
        circleGestureDetector.OnClockwiseCircleGesture.AddListener(OnStir);
        progressMeter.transform.parent.gameObject.SetActive(true);
        spoon.gameObject.SetActive(true);
    }

    protected override void FinalizeObjective()
    {
        base.FinalizeObjective();
        progressMeter.transform.parent.gameObject.SetActive(false);
        circleGestureDetector.OnClockwiseCircleGesture.RemoveListener(OnStir);
        circleGestureDetector.enabled = false;
    }

    protected override bool SuccessConditionMet()
    {
        return currentCount == requiredStirCount;
    }


    private void OnStir()
    {
        // Check if spoon is in the cookware
        if (!spoon.InCookware) { return; }

        Draggable spoonDrag = spoon.GetComponent<Draggable>();

        if (spoonDrag != null)
        {
            if (!spoonDrag.Grabbed)
                return;
        }

        currentCount++;

        if (currentCount >= stirCountLimit)
        {
            return;
        }

        // Update progress bar
        progressMeter.UpdateProgress(currentCount / stirCountLimit);

        // Image
        if (waterThickenedImage != null)
        {
            Color color = waterThickenedImage.color;
            color.a = currentCount / stirCountLimit;
            waterThickenedImage.color = color;
        }

        if (currentCount == 1)
        {
            GoToNextObjective(false);
        }
    }
}
