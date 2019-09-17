using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForScumObjective : Objective
{
    [SerializeField] private WaterScum waterScum;
    private Coroutine coroutine;

    protected override void InitializeObjective()
    {
        base.InitializeObjective();

        // Make sure required objects are active
        waterScum.transform.parent.gameObject.SetActive(true);
        waterScum.gameObject.SetActive(true);

        coroutine = StartCoroutine(waterScum.Display());
    }

    protected override void FinalizeObjective()
    {
        base.FinalizeObjective();
        StopCoroutine(coroutine);
    }

    protected override bool SuccessConditionMet()
    {
        return waterScum.Displayed;
    }
}
