using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Procedure : ScriptableObject
{
    [HideInInspector] public UnityEvent OnProcedureSuccess = new UnityEvent();
    [HideInInspector] public UnityEvent OnProcedureFail = new UnityEvent();


    private void OnEnable()
    {
        Reset();
    }

    public abstract IEnumerator Apply(PrepStation prepStation); // TODO: Move behavior to Monobehaviour scripts
    public abstract void Reset();
}
