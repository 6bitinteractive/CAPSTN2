using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable] public class TaskEvent : UnityEvent<Task> { }

[RequireComponent(typeof(Duration))]
public abstract class Task : MonoBehaviour
{
    public bool Active { get; protected set; }
    public bool Successful { get; protected set; }

    public TaskEvent OnBegin = new TaskEvent();
    public TaskEvent OnEnd = new TaskEvent();
    public TaskEvent OnSuccess = new TaskEvent();
    public TaskEvent OnFail = new TaskEvent();

    public RatingComment RatingComment;
    // TODO: Add list of flavor/effects that this task adds to the dish's flavor profile

    private Duration duration;
    protected InputHandler inputHandler;

    private void Awake()
    {
        duration = GetComponent<Duration>();
        inputHandler = SingletonManager.GetInstance<InputHandler>();
    }

    private void OnEnable()
    {
        duration.OnTimerStart.AddListener(Begin);
        duration.OnTimerEnd.AddListener(End);
    }

    private void OnDisable()
    {
        duration.OnTimerStart.RemoveListener(Begin);
        duration.OnTimerEnd.RemoveListener(End);
    }

    private void Update()
    {
        if (!Active) { return; }

        RunTask();
    }

    public void Begin()
    {
        Debug.Log("Task begun.");

        Setup();

        Active = true;
        OnBegin.Invoke(this);
    }

    public void End()
    {
        Debug.Log("Task ended.");

        Active = false;

        FinalizeTask();

        if (SuccessConditionMet())
        {
            Debug.Log("Task - Succesful - " + gameObject.name);
            Successful = true;
            OnSuccess.Invoke(this);
        }
        else
        {
            Debug.Log("Task - Failed - " + gameObject.name);
            Successful = false;
            OnFail.Invoke(this);
        }

        OnEnd.Invoke(this);
    }

    // Define how the task can be flagged as successful
    protected abstract bool SuccessConditionMet();

    // Place to do things just before starting the task
    protected virtual void Setup() { }

    // Place to do things just before ending the task, eg. calculating ratings
    protected virtual void FinalizeTask() { } // Names are hard ;o;

    // Define the task
    protected virtual void RunTask() { }
}

[System.Serializable]
public class RatingComment
{
    public string SuccessText;
    public string FailText;
}
