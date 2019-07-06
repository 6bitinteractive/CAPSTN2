using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeRating : MonoBehaviour
{
    public static int MAX_RATING = 5;
    [SerializeField] private StepRating[] stepRatings;

    public int FinalRating { get => Mathf.FloorToInt(currentSum / stepRatings.Length); }
    private int currentSum;

    private void OnEnable()
    {
        foreach (var rating in stepRatings)
            rating.OnFinalEvaluation.AddListener(AddRating);
    }

    private void OnDisable()
    {
        foreach (var rating in stepRatings)
            rating.OnFinalEvaluation.RemoveListener(AddRating);
    }

    private void AddRating(StepRating stepRating)
    {
        currentSum += stepRating.Rating;
    }
}
