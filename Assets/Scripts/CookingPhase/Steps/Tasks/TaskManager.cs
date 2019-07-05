using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TaskManager : MonoBehaviour
{
    public List<Task> Tasks;
    private List<TaskUI> taskInterfaces;
    private int currentTask;

    public UnityEvent OnAllTasksDone = new UnityEvent();

    private void Awake()
    {
        // NOTE: Ensure that the list has items or else StepRating fails to listen to the task events (race condition?)
        if (Tasks.Count == 0)
            Debug.LogError(gameObject.name + ": No item in Tasks list.");

        taskInterfaces = new List<TaskUI>();
        taskInterfaces.AddRange(GetComponentsInChildren<TaskUI>());
    }

    private void OnEnable()
    {
        foreach (var task in taskInterfaces)
            task.Duration.OnTimerEnd.AddListener(RevealTask);
    }

    private void OnDisable()
    {
        foreach (var task in taskInterfaces)
            task.Duration.OnTimerEnd.RemoveListener(RevealTask);
    }

    public void RevealTask()
    {
        StopAllCoroutines();
        StartCoroutine(Reveal());
    }

    private IEnumerator Reveal()
    {
        // Hide the previous task unless it's the first
        if (currentTask > 0)
            yield return StartCoroutine(taskInterfaces[currentTask - 1].Hide());

        // Check if all tasks are done
        if (currentTask >= taskInterfaces.Count)
        {
            OnAllTasksDone.Invoke();
            yield break;
        }

        // Reveal the task
        taskInterfaces[currentTask].Reveal();

        // Move to next task
        currentTask++;
    }
}
