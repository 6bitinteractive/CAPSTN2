using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Procedure : ScriptableObject
{
    [HideInInspector] public UnityEvent OnProcedureDone = new UnityEvent();

    public abstract void Apply(PrepStation prepStation);

    // Test; force end
    public IEnumerator End()
    {
        Debug.Log("Procedure... Done. Wait for 3s.");

        yield return new WaitForSeconds(3f);
        OnProcedureDone.Invoke();
    }
}
