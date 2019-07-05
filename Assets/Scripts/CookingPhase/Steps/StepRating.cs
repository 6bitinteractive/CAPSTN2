using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepRating : MonoBehaviour
{
    [SerializeField] private TaskManager taskManager;
    [HideInInspector] public int Rating = RecipeRating.MAX_RATING;
    [HideInInspector] public List<string> Comments = new List<string>();

    private void Awake()
    {
        if (taskManager == null)
            Debug.LogError(gameObject.name + "taskManager is null.");
    }

    private void OnEnable()
    {
        foreach (var task in taskManager.Tasks)
            task.OnEnd.AddListener(Evaluate);
    }

    private void OnDisable()
    {
        foreach (var task in taskManager.Tasks)
            task.OnEnd.RemoveListener(Evaluate);
    }

    private void Evaluate(Task task)
    {
        Rating += task.Successful ? 0 : -1; // If the task failed, rating decrease by 1
        Rating = Mathf.Clamp(Rating, 0, RecipeRating.MAX_RATING); // In case tasks have more than five steps and player fails more than five times
        Comments.Add(task.Successful ? task.RatingComment.SuccessText : task.RatingComment.FailText);

        Debug.Log(task.gameObject.name + "Added Comment: " + (task.Successful ? task.RatingComment.SuccessText : task.RatingComment.FailText));
    }
}
