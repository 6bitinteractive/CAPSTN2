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
    }

    public void EndCurrentObjective()
    {
        Objectives[currentObjective].End();
    }
}
