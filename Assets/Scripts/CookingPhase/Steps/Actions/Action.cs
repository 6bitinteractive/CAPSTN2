using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Action : MonoBehaviour
{
    public string Instruction;
    public bool Active;

    public UnityEvent OnEnd = new UnityEvent();
    public UnityEvent OnSuccess = new UnityEvent();
    public UnityEvent OnFail = new UnityEvent();

    public abstract void Begin();
    public abstract void ResetAction();
}
