using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable] public class ObjectiveEvent : UnityEvent<Objective> { }

public abstract class Objective : MonoBehaviour
{
    [SerializeField] protected string descriptionText;

    public string Description => descriptionText;
    public bool Successful { get; protected set; }
    public bool Active { get; protected set; }

    public ObjectiveEvent OnBegin = new ObjectiveEvent();
    public ObjectiveEvent OnEnd = new ObjectiveEvent();
    public ObjectiveEvent OnSuccess = new ObjectiveEvent();
    public ObjectiveEvent OnFail = new ObjectiveEvent();

    public ObjectiveEvent OnPerfect = new ObjectiveEvent();
    public ObjectiveEvent OnUnder = new ObjectiveEvent();
    public ObjectiveEvent OnOver = new ObjectiveEvent();

    protected static Kitchen kitchen;

    protected virtual void Awake() { }
    protected virtual void OnEnable() { }
    protected virtual void OnDisable() { }
    protected virtual void OnDestroy() { }

    private void Start()
    {
        // Begin objective as soon as the object has been enabled
        Begin();
    }

    private void Update()
    {
        if (!Active) { return; }

        RunObjective();
    }

    public void Begin()
    {
        Debug.Log("Objective - Initialized");
        kitchen = SingletonManager.GetInstance<Kitchen>(); // TODO: Iffy to use the singleton manager; issues with race condition
        InitializeObjective();

        Active = true;
        OnBegin.Invoke(this);
    }

    public void End()
    {
        Debug.Log("Objective - End");
        Active = false;

        FinalizeObjective();

        Successful = SuccessConditionMet();
        if (Successful)
        {
            Debug.Log("Objective - Successful");
            OnSuccess.Invoke(this);
        }
        else
        {
            Debug.Log("Objective - Failed");
            OnFail.Invoke(this);
        }

        OnEnd.Invoke(this);
    }

    // Define how the objective can be flagged successful
    protected abstract bool SuccessConditionMet();

    // Define the actual objective
    protected virtual void RunObjective() { }

    // Stuff to do before beginning
    protected virtual void InitializeObjective() { }

    // Stuff to do before ending
    protected virtual void FinalizeObjective() { }
}

public enum ObjectiveState
{
    Perfect, Under, Over
}
