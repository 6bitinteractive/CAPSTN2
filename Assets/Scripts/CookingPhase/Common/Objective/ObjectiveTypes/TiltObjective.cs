using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TiltDetector))]

public class TiltObjective : Objective
{
    [SerializeField] private TiltDirection requiredTiltDirection;
    [SerializeField] private DialogueHint dialogueHint;
    [SerializeField] private ObjectiveState perfectState = new ObjectiveState(ObjectiveState.Status.Perfect);

    private TiltDetector tiltDetector;
    private TiltDirection playerTilt = TiltDirection.None;

    protected override void Awake()
    {
        base.Awake();
        tiltDetector = GetComponent<TiltDetector>();
        tiltDetector.enabled = false;

        // Setup objectives
        // Add to list
        ObjectiveStates.Add(perfectState);

        // Define condition
        perfectState.HasBeenReached = () => SuccessConditionMet();
    }

    protected override void InitializeObjective()
    {
        base.InitializeObjective();
        tiltDetector.OnTilt.AddListener(CheckPlayerTiltInput);
        tiltDetector.enabled = true;
        InitializeTiltType();
        perfectState.OnStateReached.AddListener((x) => GoToNextObjective(true));

        if (dialogueHint.dialogueText != string.Empty)
            SingletonManager.GetInstance<DialogueHintManager>().Show(dialogueHint);
    }

    protected override void FinalizeObjective()
    {
        base.FinalizeObjective();
        tiltDetector.OnTilt.RemoveListener(CheckPlayerTiltInput);
        tiltDetector.enabled = false;
    }

    private void CheckPlayerTiltInput(TiltDirection direction)
    {
        playerTilt = direction;
    }

    protected override bool SuccessConditionMet()
    {
        return playerTilt == requiredTiltDirection;
    }

    private void InitializeTiltType()
    {
        if (requiredTiltDirection == TiltDirection.Up || requiredTiltDirection == TiltDirection.Down)
            tiltDetector.TiltType = TiltType.Vertical;
        else
            tiltDetector.TiltType = TiltType.Horizontal;
    }
}

