using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatingsManager : MonoBehaviour
{
    private Dictionary<SceneData, int> stageRatings = new Dictionary<SceneData, int>();

    private void OnEnable()
    {
        SingletonManager.Register<RatingsManager>(this);
    }

    private void OnDisable()
    {
        SingletonManager.UnRegister<RatingsManager>();
    }

    public void UpdateStageRating(SceneData stage, int rating)
    {
        // Check if there's already a stored rating
        if (stageRatings.TryGetValue(stage, out int storedRating))
        {
            // Check if new rating is better
            if (rating > storedRating)
                stageRatings[stage] = rating; // Update to new "highscore"
        }
        else // Add as new key-value pair
        {
            stageRatings.Add(stage, rating);
        }
    }

    public int LoadStageRating(SceneData stage)
    {
        if (stageRatings.TryGetValue(stage, out int rating))
            return rating;
        else
            return 0;
    }
}
