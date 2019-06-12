using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriedRiceAction : Action
{
    public override void Begin()
    {
        OnBegin.Invoke(this);
    }

    public override void ResetAction()
    {
    }

    public override bool SuccessConditionMet()
    {
        return true;
    }
}
