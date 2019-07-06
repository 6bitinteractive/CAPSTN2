using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable] public class StepRatingEvent : UnityEvent<StepRating> { }

public class StepRating : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] private TextMeshProUGUI ratingText;
    [SerializeField] private TextMeshProUGUI commentText;

    [Space]

    [SerializeField] private TaskManager taskManager;
    [HideInInspector] public int Rating = RecipeRating.MAX_RATING;
    [HideInInspector] public List<string> Comments = new List<string>();

    public StepRatingEvent OnFinalEvaluation = new StepRatingEvent();

    private void Awake()
    {
        if (taskManager == null)
            Debug.LogError(gameObject.name + "taskManager is null.");
    }

    private void OnEnable()
    {
        taskManager.OnAllTasksDone.AddListener(DisplayEvaluation);

        foreach (var task in taskManager.Tasks)
            task.OnEnd.AddListener(Evaluate);
    }

    private void OnDisable()
    {
        taskManager.OnAllTasksDone.RemoveListener(DisplayEvaluation);

        foreach (var task in taskManager.Tasks)
            task.OnEnd.RemoveListener(Evaluate);
    }

    private void Evaluate(Task task)
    {
        Rating += task.Successful ? 0 : -1; // If the task failed, rating decrease by 1
        Rating = Mathf.Clamp(Rating, 0, RecipeRating.MAX_RATING); // In case tasks have more than the number of maxRating and player fails more than maxRating
        Comments.Add(task.Successful ? task.RatingComment.SuccessText : task.RatingComment.FailText);

        Debug.Log(task.gameObject.name + "Added Comment: " + (task.Successful ? task.RatingComment.SuccessText : task.RatingComment.FailText));
    }

    private void DisplayEvaluation()
    {
        ratingText.text = string.Format("RATING: {0} / {1}", Rating.ToString(), RecipeRating.MAX_RATING);
        commentText.text = string.Format("Comments: {0}", string.Join(" ", Comments));
        OnFinalEvaluation.Invoke(this);
    }
}
