using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectiveManager : MonoBehaviour
{
    public List<Objective> Objectives { get; private set; }
    private int currentObjective;

    public UnityEvent OnAllObjectivesDone = new UnityEvent();

    private void Awake()
    {
        Objectives = new List<Objective>();
        Objectives.AddRange(GetComponentsInChildren<Objective>());

        foreach (var objective in Objectives)
            objective.gameObject.SetActive(false);
    }

    private void Start()
    {
        // Start first objective
        Objectives[0].gameObject.SetActive(true);
    }

    public void EndCurrentObjective()
    {
        Objectives[currentObjective].End();
        currentObjective++;

        if (currentObjective >= Objectives.Count)
        {
            Debug.Log("All objectives done.");
            OnAllObjectivesDone.Invoke();
            return;
        }

        Objectives[currentObjective].gameObject.SetActive(true);
    }
}
