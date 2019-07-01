using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TaskManager : MonoBehaviour
{
    private List<TaskUI> tasks;
    private int currentTask;

    public UnityEvent OnAllTasksDone = new UnityEvent();

    private void Awake()
    {
        tasks = new List<TaskUI>();
        tasks.AddRange(GetComponentsInChildren<TaskUI>());
    }

    private void OnEnable()
    {
        foreach (var task in tasks)
            task.GetComponent<Duration>().OnTimerEnd.AddListener(RevealTask);
    }

    private void OnDisable()
    {
        foreach (var task in tasks)
            task.GetComponent<Duration>().OnTimerEnd.RemoveListener(RevealTask);
    }

    public void RevealTask()
    {
        // Hide the previous task unless it's the first
        if (currentTask > 0)
            tasks[currentTask - 1].gameObject.SetActive(false);

        // Check if all tasks are done
        if (currentTask >= tasks.Count)
        {
            OnAllTasksDone.Invoke();
            return;
        }

        // Reveal the task
        tasks[currentTask].Reveal();

        // Move to next task
        currentTask++;
    }
}
