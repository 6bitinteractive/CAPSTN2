using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Prompt : PoolObject
{
    [HideInInspector] public UnityEvent OnSuccessfulInput = new UnityEvent();
    [HideInInspector] public UnityEvent OnFailedInput = new UnityEvent();

    private static InputListener inputListener;

    protected virtual void Awake()
    {
        inputListener = SingletonManager.GetInstance<InputListener>();
    }

    protected virtual void Start()
    {
        OnSuccessfulInput.AddListener(inputListener.InvokeOnSuccess);
        OnFailedInput.AddListener(inputListener.InvokeOnFail);
    }
}
