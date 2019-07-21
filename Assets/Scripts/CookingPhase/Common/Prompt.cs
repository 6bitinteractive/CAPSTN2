using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Prompt : PoolObject
{
    [HideInInspector] public UnityEvent OnSuccessfulInput = new UnityEvent();
    [HideInInspector] public UnityEvent OnFailedInput = new UnityEvent();

    protected static InputListener inputListener;

    protected virtual void Awake()
    {
        inputListener = SingletonManager.GetInstance<InputListener>();
    }

    protected virtual void Start()
    {
        OnSuccessfulInput.AddListener(inputListener.InvokeOnSuccess);
        OnSuccessfulInput.AddListener(inputListener.InvokeOnInputEnd);
        OnFailedInput.AddListener(inputListener.InvokeOnFail);
        OnFailedInput.AddListener(inputListener.InvokeOnInputEnd);
    }

    protected virtual void OnDestroy()
    {
        OnSuccessfulInput.RemoveAllListeners();
        OnFailedInput.RemoveAllListeners();
    }
}
