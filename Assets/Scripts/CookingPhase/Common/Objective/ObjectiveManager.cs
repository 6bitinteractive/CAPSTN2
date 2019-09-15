using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    public List<Objective> Objectives { get; private set; }
    private int currentObjective;

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
        if (currentObjective >= Objectives.Count) { return; }
        else Objectives[currentObjective].gameObject.SetActive(true);
    }
}
