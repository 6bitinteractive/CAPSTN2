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

        // Only get the direct descendants
        Objectives = Objectives.FindAll((x) => x.transform.parent == transform);

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
        // Is this an ObjectiveGroup, i.e. we will need to go through its sub-objectives first
        if (typeof(ObjectiveGroup).IsAssignableFrom(Objectives[currentObjective].GetType()))
        {
            Debug.Log("Objective Group...");
            if (!((ObjectiveGroup)Objectives[currentObjective]).IsComplete) // As long as this objective hasn't ran out of sub-objectives to do...
            {
                ((ObjectiveGroup)Objectives[currentObjective]).MoveToNextObjective(); // ...just keep going to the next
                return;
            }
        }

        Objectives[currentObjective].End();
        Debug.Log("Current objective: " + currentObjective);
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
