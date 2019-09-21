using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// int: rating
[System.Serializable] public class RatingEvent : UnityEvent<int> { }

[RequireComponent(typeof(Kitchen))]
public class RatingCalculator : MonoBehaviour
{
    [SerializeField] private ObjectiveManager objectiveManager;
    [SerializeField][Range(0, 1)] private float maxOneStar = 0.5f, maxTwoStar = 0.7f;

    public RatingEvent OnFinalRatingCalculated = new RatingEvent();

    private Dictionary<PromptRating, int> promptRatings = new Dictionary<PromptRating, int>();
    private RatingsManager ratingsManager;
    private SceneData stage;

    private void OnEnable()
    {
        objectiveManager.OnAllObjectivesDone.AddListener(EvaluateFinalRating);
    }

    private void OnDisable()
    {
        objectiveManager.OnAllObjectivesDone.RemoveListener(EvaluateFinalRating);
    }

    private void Start()
    {
        ratingsManager = SingletonManager.GetInstance<RatingsManager>();
        stage = GetComponent<Kitchen>().StageScene;
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
        float totalCorrect = (float)objectiveManager.Objectives.FindAll(x => x.Successful).Count / (float)objectiveManager.Objectives.Count;
        Debug.LogFormat("Correct %: {0}", totalCorrect);

        // Calculation
        int rating = 1;
        if (totalCorrect > maxTwoStar)
            rating = 3;
        else if (totalCorrect <= maxTwoStar && totalCorrect > maxOneStar)
            rating = 2;

        Debug.LogFormat("Rating: {0}", rating);

        ratingsManager.UpdateStageRating(stage, rating); // Store rating (persistent/session)
        OnFinalRatingCalculated.Invoke(rating);
    }
}
