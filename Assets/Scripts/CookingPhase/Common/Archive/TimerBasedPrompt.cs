﻿using System;
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

        // Note to self: This is here because if I add a RemoveListener in OnDisable, the AddListener in OnEnable doesn't seem to be listening again to the event
        // OTOH, just keeping the AddListener in OnEnable (without removing it in OnDisable) adds another listener to the event(?) and thus runs the callback(?) twice :(
        markerController.OnMarkerStop.AddListener(Evaluate);
    }

    protected override void Start()
    {
        base.Start();
        OnEvaluatePromptRating.AddListener(inputListener.InvokeOnEvaluatePrompt);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        OnEvaluatePromptRating.RemoveListener(inputListener.InvokeOnEvaluatePrompt);
    }

    public void Evaluate(MarkerController marker)
    {
        if (marker.CurrentNormalizedPosition >= 1f)
            promptRating = PromptRating.Miss;
        else if (marker.CurrentNormalizedPosition <= promptAreas.Perfect.fillAmount)
            promptRating = PromptRating.Perfect;
        else if (marker.CurrentNormalizedPosition <= promptAreas.Great.fillAmount)
            promptRating = PromptRating.Great;
        else if (marker.CurrentNormalizedPosition <= promptAreas.Good.fillAmount)
            promptRating = PromptRating.Good;
        else if (marker.CurrentNormalizedPosition <= promptAreas.Bad.fillAmount)
            promptRating = PromptRating.Bad;
        else if (marker.CurrentNormalizedPosition < promptAreas.Awful.fillAmount)
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
