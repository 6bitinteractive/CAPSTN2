using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable] public class ActionEvent : UnityEvent<Action> { }

public abstract class Action : MonoBehaviour
{
    public string Instruction;
    public bool Successful { get; protected set; }
    public bool Active { get; protected set; }

    public ActionEvent OnBegin = new ActionEvent();
    public ActionEvent OnEnd = new ActionEvent();
    public ActionEvent OnSuccess = new ActionEvent();
    public ActionEvent OnFail = new ActionEvent();

    public abstract void Begin(); // What gets called to start the Action
    public abstract void ResetAction(); // Reset to default values
    public abstract bool SuccessConditionMet(); // Define how the Action can be flagged as successful
}
