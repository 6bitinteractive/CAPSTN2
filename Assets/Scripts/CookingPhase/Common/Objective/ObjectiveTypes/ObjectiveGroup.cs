using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveGroup : Objective
{
    public bool IsComplete { get; protected set; }
    public List<Objective> objectives = new List<Objective>();
    private int currentObjective;

    protected override void Awake()
    {
        base.Awake();
        objectives.AddRange(GetComponentsInChildren<Objective>());

        // Apparently, the above GetComponenetsInChildren includes the parent @_@
        // ... so we remove the parent (this object) from the list... orz
        objectives.Remove(this);

        foreach (var objective in objectives)
        {
            // Make sure objectives don't start all at once
            objective.BeginAtStart = false;
        }
    }

    protected override void InitializeObjective()
    {
        base.InitializeObjective();
        objectives[currentObjective].Begin();

        // Listen when the last objective ends
        objectives[objectives.Count - 1].OnEnd.AddListener(x => IsComplete = true);
    }

    protected override bool SuccessConditionMet()
    {
        // Objective can't be successful if there's at least one objective that's unsuccessful
        return !objectives.Exists(x => !x.Successful);
    }

    public void MoveToNextObjective()
    {
        // End current objective
        objectives[currentObjective].End();

        // Move to next
        currentObjective++;

        if (currentObjective < objectives.Count)
            objectives[currentObjective].Begin(); // Begin the next sub-objective
    }
}
