using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable] public class ObjectiveEvent : UnityEvent<Objective> { }
[System.Serializable] public class ObjectiveStateEvent : UnityEvent<ObjectiveState> { }

public abstract class Objective : MonoBehaviour
{
    [SerializeField][TextArea(1, 3)] protected string descriptionText;

    public string Description => descriptionText;
    public List<ObjectiveState> ObjectiveStates { get; protected set; }
    public bool Successful { get; protected set; }
    public bool Active { get; protected set; }

    public ObjectiveEvent OnBegin = new ObjectiveEvent();
    public ObjectiveEvent OnEnd = new ObjectiveEvent();
    public ObjectiveEvent OnSuccess = new ObjectiveEvent();
    public ObjectiveEvent OnFail = new ObjectiveEvent();

    protected static Kitchen kitchen;

    protected virtual void Awake() { }
    protected virtual void OnEnable() { }
    protected virtual void OnDisable() { }
    protected virtual void OnDestroy() { }

    private void Start()
    {
        ObjectiveStates = new List<ObjectiveState>();

        // Begin objective as soon as the object has been enabled
        Begin();
    }

    private void Update()
    {
        if (!Active) { return; }

        // Check states if there are any
        if (ObjectiveStates.Count > 0)
            EvaluateState();

        RunObjective();
    }

    public void Begin()
    {
        Debug.LogFormat("{0} - Start", gameObject.name);
        kitchen = SingletonManager.GetInstance<Kitchen>(); // FIX: Iffy to use the singleton manager; issues with race condition
        InitializeObjective();

        Active = true;
        OnBegin.Invoke(this);
    }

    public void End()
    {
        Debug.LogFormat("{0} - End", gameObject.name);
        Active = false;

        FinalizeObjective();

        Successful = SuccessConditionMet();
        if (Successful)
        {
            Debug.LogFormat("{0} - Successful", gameObject.name);
            OnSuccess.Invoke(this);
        }
        else
        {
            Debug.LogFormat("{0} - Failed", gameObject.name);
            OnFail.Invoke(this);
        }

        OnEnd.Invoke(this);
    }

    // Define how the objective can be flagged successful
    protected abstract bool SuccessConditionMet();

    // Define the actual objective; called in Update()
    protected virtual void RunObjective() { }

    // Stuff to do before beginning (eg. instantiate objects, initialize variables, etc)
    protected virtual void InitializeObjective() { }

    // Stuff to do before ending (eg. do some calculations before checking success condition)
    protected virtual void FinalizeObjective() { }

    // Evaluate each state
    private void EvaluateState()
    {
        foreach (var state in ObjectiveStates)
        {
            // Just continue to the next state if the currently checked state is not active (ie. it's already past that state)
            if (!state.Active) { continue; }

            if (state.HasBeenReached())
            {
                Debug.LogFormat("{0} is at {1} State", gameObject.name, state.StatusType);
                state.Active = false;
                state.OnStateReached.Invoke(state);
                return;
            }
        }

        return;
    }
}

// Note: Current implementation assumes once a state has been reached, it's never going to be checked again if it recurs
[System.Serializable]
public class ObjectiveState
{
    public Status StatusType { get; }
    public System.Func<bool> HasBeenReached { get; set; }
    public bool Active { get; set; }

    [TextArea(1, 3)] public string dialogueText;

    public ObjectiveStateEvent OnStateReached = new ObjectiveStateEvent();

    public ObjectiveState(Status _status)
    {
        StatusType = _status;
        Active = true;
    }

    public enum Status
    {
        Perfect, Under, Over
    }
}
