using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EffectsHandler : MonoBehaviour
{
    [SerializeField] private ObjectiveManager objectiveManager;
    [SerializeField] private ParticleSystem[] particleSystems;

    public UnityEvent OnPerfect = new UnityEvent();

    private void Start()
    {
        foreach (var objective in objectiveManager.Objectives)
            objective.OnBegin.AddListener(ListenToStateChanges);
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

            OnPerfect.Invoke();
        }
    }
}
