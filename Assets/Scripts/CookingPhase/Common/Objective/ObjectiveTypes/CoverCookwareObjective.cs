using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverCookwareObjective : Objective
{
    [SerializeField] private LidHandler lid;
    [SerializeField] private Sprite timeskipSprite;

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

    protected override void FinalizeObjective()
    {
        base.FinalizeObjective();
        SingletonManager.GetInstance<Timeskip>().Show(timeskipSprite);
    }

    protected override bool SuccessConditionMet()
    {
        return lid.IsCoveringCookware;
    }
}
