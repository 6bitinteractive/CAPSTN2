using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Step : MonoBehaviour
{
    [SerializeField] private List<Action> actions;

    private int currentAction = -1;

    private void OnEnable()
    {
        foreach (var action in actions)
        {
            action.ResetAction();
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

    public void MoveToNextAction()
    {
        currentAction++;

        if (currentAction < actions.Count)
        {
            actions[2].Begin();
        }
    }
}
