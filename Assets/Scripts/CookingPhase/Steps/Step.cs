using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Step : MonoBehaviour
{
    public List<Action> actions;

    public UnityEvent OnEnd = new UnityEvent();

    private int currentAction = -1;

    private void OnEnable()
    {
        foreach (var action in actions)
        {
            action.ResetAction();
            action.OnEnd.AddListener(MoveToNextAction);
        }
    }

    private void Start()
    {
        MoveToNextAction();
    }

    public void Restart()
    {
        currentAction = 0;
    }

    public void MoveToNextAction(Action action = null)
    {
        currentAction++;

        if (currentAction < actions.Count)
        {
            actions[currentAction].Begin();
        }
        else
        {
            OnEnd.Invoke();
        }
    }
}
