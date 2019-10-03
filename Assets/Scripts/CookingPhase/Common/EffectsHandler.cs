﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsHandler : MonoBehaviour
{
    [SerializeField] private ObjectiveManager objectiveManager;
    [SerializeField] private ParticleSystem[] particleSystems;

    private void Start()
    {
        foreach (var objective in objectiveManager.Objectives)
        {
            // Is this an ObjectiveGroup, i.e. we will need to go through its sub-objectives first
            if (typeof(ObjectiveGroup).IsAssignableFrom(objective.GetType()))
            {
                ObjectiveGroup og = (ObjectiveGroup)objective;
                foreach (var item in og.objectives)
                {
                    item.OnBegin.AddListener(ListenToStateChanges);
                }
            }

            objective.OnBegin.AddListener(ListenToStateChanges);
        }
    }

    private void ListenToStateChanges(Objective objective)
    {
        foreach (var objectiveState in objective.ObjectiveStates)
            objectiveState.OnStateReached.AddListener(PlayParticleSystem);
    }

    private void PlayParticleSystem(ObjectiveState objectiveState)
    {
        if (objectiveState.StatusType == ObjectiveState.Status.Perfect)
        {
            Debug.Log("Play particle effects");
            foreach (var particleSystem in particleSystems)
                particleSystem.Play();
        }
    }
}
