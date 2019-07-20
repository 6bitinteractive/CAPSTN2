using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable] public class OnEvaluatePromptRating : UnityEvent<PromptRating> { }

public class TimerBasedPrompt : Prompt
{
    [SerializeField] private PromptAreas promptAreas;
    [SerializeField] private MarkerController markerController;

    public PromptRating promptRating { get; private set; }
    public OnEvaluatePromptRating OnEvaluatePromptRating = new OnEvaluatePromptRating();

    protected override void Awake()
    {
        base.Awake();
        markerController.OnMarkerStop.AddListener(Evaluate);
    }

    public void Evaluate(MarkerController rotatingMarker)
    {
        if (rotatingMarker.CurrentNormalizedPosition >= 1f)
            promptRating = PromptRating.Miss;
        else if (rotatingMarker.CurrentNormalizedPosition <= promptAreas.Perfect.fillAmount)
            promptRating = PromptRating.Perfect;
        else if (rotatingMarker.CurrentNormalizedPosition <= promptAreas.Great.fillAmount)
            promptRating = PromptRating.Great;
        else if (rotatingMarker.CurrentNormalizedPosition <= promptAreas.Good.fillAmount)
            promptRating = PromptRating.Good;
        else if (rotatingMarker.CurrentNormalizedPosition <= promptAreas.Bad.fillAmount)
            promptRating = PromptRating.Bad;
        else if (rotatingMarker.CurrentNormalizedPosition < promptAreas.Awful.fillAmount)
            promptRating = PromptRating.Awful;
        else
            Debug.LogError("No rating");

        Debug.Log("Prompt rating: " + promptRating);

        if (promptRating == PromptRating.Miss)
            OnFailedInput.Invoke();
        else
            OnSuccessfulInput.Invoke();

        OnEvaluatePromptRating.Invoke(promptRating);
    }
}

public enum PromptRating
{
    Miss, Awful, Bad, Good, Great, Perfect
}

[System.Serializable]
public class PromptAreas
{
    public Image Awful, Bad, Good, Great, Perfect;
}
