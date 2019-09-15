using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverCookwareObjective : Objective
{
    [SerializeField] private LidHandler lid;

    protected override void Awake()
    {
        if (lid == null)
            Debug.LogError("Assign lid variable.");
    }

    protected override void InitializeObjective()
    {
        base.InitializeObjective();

        lid.gameObject.SetActive(true);
    }

    protected override bool SuccessConditionMet()
    {
        return lid.IsCoveringCookware;
    }
}
