using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputListener : MonoBehaviour
{
    public UnityEvent OnSuccess = new UnityEvent();
    public UnityEvent OnFail = new UnityEvent();
    public UnityEvent OnInputEnd = new UnityEvent(); // Doesn't matter if player failed or succeeded as long as there was an attempt to do the prompt

    private void Awake()
    {
        SingletonManager.Register<InputListener>(this);
    }

    public void InvokeOnSuccess()
    {
        OnSuccess.Invoke();
    }

    public void InvokeOnFail()
    {
        OnFail.Invoke();
    }

    public void InvokeOnInputEnd()
    {
        OnInputEnd.Invoke();
    }
}
