using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRatings : MonoBehaviour
{
    [SerializeField] private SceneData stage;
    private RatingsManager ratingsManager;

    private void Start()
    {
        ratingsManager = SingletonManager.GetInstance<RatingsManager>();
    }

    public void SaveRating()
    {
        ratingsManager.UpdateStageRating(stage, 3);
    }
}
