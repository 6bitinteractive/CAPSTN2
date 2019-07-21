using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// FIX this mess... maybe :/
public class InputListener : MonoBehaviour
{
    [Header("Generic")]
    public UnityEvent OnSuccess = new UnityEvent();
    public UnityEvent OnFail = new UnityEvent();
    public UnityEvent OnInputEnd = new UnityEvent(); // Doesn't matter if player failed or succeeded as long as there was an attempt to do the prompt

    [Header("Specific")]
    public OnEvaluatePromptRating OnEvaluatePrompt = new OnEvaluatePromptRating();

    private void OnEnable()
    {
        SingletonManager.Register<InputListener>(this);
    }

    private void OnDisable()
    {
        SingletonManager.UnRegister<InputListener>();
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

    public void InvokeOnEvaluatePrompt(PromptRating promptRating)
    {
        OnEvaluatePrompt.Invoke(promptRating);
    }
}
