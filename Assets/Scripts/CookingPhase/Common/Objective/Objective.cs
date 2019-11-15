using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable] public class ObjectiveEvent : UnityEvent<Objective> { }
[System.Serializable] public class ObjectiveStateEvent : UnityEvent<ObjectiveState> { }

public abstract class Objective : MonoBehaviour
{
    [SerializeField] protected bool startOnEnable = true;
    [SerializeField] [TextArea(1, 3)] protected string descriptionText;

    public string Description => descriptionText;
    public bool Successful { get; protected set; }
    public bool Active { get; protected set; }
    [HideInInspector] public List<ObjectiveState> ObjectiveStates = new List<ObjectiveState>(); // NOTE: See PourObjective.cs for implementation of ObjectiveStates

    public ObjectiveEvent OnBegin = new ObjectiveEvent();
    public ObjectiveEvent OnEnd = new ObjectiveEvent();
    public ObjectiveEvent OnSuccess = new ObjectiveEvent();
    public ObjectiveEvent OnFail = new ObjectiveEvent();

    // NOTE: These are kind of hacks since there's no pattern as to when the Next button will end an objective or if it's just automatic
    public ObjectiveEvent OnReadyForNext = new ObjectiveEvent(); // Allow player to move to next objective (i.e. show Next button)
    public ObjectiveEvent OnAutomaticallyGoToNext = new ObjectiveEvent();

    protected static Kitchen kitchen;

    protected virtual void Awake() { }
    protected virtual void OnEnable() { }
    protected virtual void OnDisable() { }
    protected virtual void OnDestroy() { }

    private void Start()
    {
        // Begin objective as soon as the object has been enabled
        if (startOnEnable)
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

        PostFinalizeObjective();

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

    // Stuff to do after finalizing and evaluating if objective was done successfully but just before ending
    protected virtual void PostFinalizeObjective() { }

    // Go to next objective (via next button or automatically)
    protected virtual void GoToNextObjective(bool automatic = false) // False = a Next button will be shown to the player and he has the autonomy to end at his own time
    {
        if (automatic)
            OnAutomaticallyGoToNext.Invoke(this);
        else
            OnReadyForNext.Invoke(this);
    }

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
    }
}

// NOTE: Current implementation assumes once a state has been reached, it's never going to be checked again if it recurs
[System.Serializable]
public class ObjectiveState // NOTE: See PourObjective.cs for implementation of ObjectiveStates
{
    public Status StatusType { get; }
    public System.Func<bool> HasBeenReached { get; set; }
    public bool Active { get; set; }

    public DialogueHint dialogueHint;
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
