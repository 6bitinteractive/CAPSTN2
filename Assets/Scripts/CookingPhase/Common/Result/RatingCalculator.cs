using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// int: rating
[System.Serializable] public class RatingEvent : UnityEvent<int> { }

[RequireComponent(typeof(Step))]
public class RatingCalculator : MonoBehaviour
{
    public RatingEvent OnFinalRatingCalculated = new RatingEvent();
    private Dictionary<PromptRating, int> promptRatings = new Dictionary<PromptRating, int>();

    private RatingsManager ratingsManager;
    private SceneData stage;

    private void Start()
    {
        ratingsManager = SingletonManager.GetInstance<RatingsManager>();
        stage = GetComponent<Step>().StageScene;
    }

    public void IncreasePromptRatingCount(PromptRating promptRating)
    {
        if (promptRatings.TryGetValue(promptRating, out int count))
            promptRatings[promptRating]++;
        else
            promptRatings.Add(promptRating, 1);

        Debug.Log(promptRating + " count: " + promptRatings[promptRating]);
    }

    public void EvaluateFinalRating()
    {
        // TODO: calculation
        int rating = 2;

        ratingsManager.UpdateStageRating(stage, rating); // Store rating (persistent/session)
        OnFinalRatingCalculated.Invoke(rating);
    }
}
