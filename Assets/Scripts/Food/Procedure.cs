using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Procedure : ScriptableObject
{
    [HideInInspector] public UnityEvent OnProcedureDone = new UnityEvent(); // not necessary? process box should handle when a procedure would end

    private void OnEnable()
    {
        Reset();
    }

    public abstract IEnumerator Apply(PrepStation prepStation);
    public abstract void Reset();
}
