using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable] public class StepEvent : UnityEvent<Step> { }

public class Step : MonoBehaviour
{
    public List<Action> Actions;
    public bool Successful { get { return !Actions.Exists(x => !x.Successful) || timeRanOut; } } // If there's one fail, the whole step is not flagged as successful

    public StepEvent OnBegin = new StepEvent();
    public StepEvent OnEnd = new StepEvent();
    public StepEvent OnSuccess= new StepEvent();
    public StepEvent OnFail = new StepEvent();

    private int currentAction = -1;
    private bool timeRanOut;

    // TODO: these shouldn't be part of this class?
    [Header("UI")]
    [SerializeField] private ResultsPanel ResultsPanel;
    [SerializeField] private InstructionsUI InstructionsPanel;

    private void OnEnable()
    {
        foreach (var action in Actions)
        {
            action.ResetAction();
            action.OnEnd.AddListener(MoveToNextAction);
        }

        OnBegin.AddListener(HideResultsPanel);
        OnBegin.AddListener(ShowInstructionsPanel);
        OnEnd.AddListener(ShowResultsPanel);
        OnEnd.AddListener(HideInstructionsPanel);
    }

    private void OnDisable()
    {
        foreach (var action in Actions)
            action.OnEnd.RemoveListener(MoveToNextAction);

        OnBegin.RemoveListener(HideResultsPanel);
        OnBegin.RemoveListener(ShowInstructionsPanel);
        OnEnd.RemoveListener(ShowResultsPanel);
        OnEnd.RemoveListener(HideInstructionsPanel);
    }

    private void Start()
    {
        OnBegin.Invoke(this);
        MoveToNextAction(Actions[0]);
    }

    public void Restart()
    {
        currentAction = 0;
    }

    public void MoveToNextAction(Action action)
    {
        currentAction++;

        if (currentAction < Actions.Count)
        {
            Actions[currentAction].Begin();
            Debug.Log("Current action: " + Actions[currentAction].gameObject.name);
        }
        else
        {
            if (Successful)
                OnSuccess.Invoke(this);
            else
                OnFail.Invoke(this);

            OnEnd.Invoke(this);
        }
    }

    private void ShowResultsPanel(Step step)
    {
        ResultsPanel.gameObject.SetActive(true);
        ResultsPanel.ShowResult(this);
    }

    private void HideResultsPanel(Step step)
    {
        ResultsPanel.gameObject.SetActive(false);
    }

    private void HideInstructionsPanel(Step arg0)
    {
        InstructionsPanel.gameObject.SetActive(false);
    }

    private void ShowInstructionsPanel(Step arg0)
    {
        InstructionsPanel.gameObject.SetActive(true);
    }
}
