using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputListener : MonoBehaviour
{
    public UnityEvent OnSuccess = new UnityEvent();
    public UnityEvent OnFail = new UnityEvent();

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
}
