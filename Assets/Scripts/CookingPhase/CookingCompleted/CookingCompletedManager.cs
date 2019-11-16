using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(SceneLoader))]

public class CookingCompletedManager : MonoBehaviour
{
    [Header("TEST")]
    [SerializeField] private bool test;
    [SerializeField] private Recipe testRecipe;

    [Header("Rating")]
    [SerializeField] private int minRating = 1;
    [SerializeField] private int maxRating = 3;

    [Header("Dish")]
    [Tooltip("In proper order.")]
    [SerializeField] private List<Image> dishImages;
    [SerializeField] private Image dishName;

    [Header("Audio")]
    [SerializeField] private AudioClip successSfx;
    [SerializeField] private AudioClip failSfx;

    private StageTracker stageTracker;
    private RatingsManager ratingsManager;
    private AudioSource audioSource;
    private SceneLoader sceneLoader;

    private void Start()
    {
        stageTracker = SingletonManager.GetInstance<StageTracker>();
        ratingsManager = SingletonManager.GetInstance<RatingsManager>();
        audioSource = GetComponent<AudioSource>();
        sceneLoader = GetComponent<SceneLoader>();

        if (test)
            DetermineDisplay(testRecipe);
        else
            DetermineDisplay(stageTracker.RecentCompletedRecipe);
    }

    public void LoadNextScene()
    {
        if (test)
            sceneLoader.LoadScene(testRecipe.PostCookingScene);
        else
            sceneLoader.LoadScene(stageTracker.RecentCompletedRecipe.PostCookingScene);
    }

    private void DetermineDisplay(Recipe recipe)
    {
        float avg = GetAverage(recipe);

        // Set image, audio
        // Fail
        if (avg <= minRating)
        {
            for (int i = 0, j = 0; i < dishImages.Count; i++, j++)
            {
                dishImages[i].sprite = recipe.FailedFinalDishSequence[j];
                audioSource.clip = failSfx;
            }
        }
        else // Success
        {
            for (int i = 0, j = 0; i < dishImages.Count; i++, j++)
            {
                dishImages[i].sprite = recipe.SuccessfulFinalDishSequence[j];
                audioSource.clip = successSfx;
            }
        }

        // Set dish name
        dishName.sprite = recipe.DishNameImage;

        // Play audio
        if (audioSource.clip != null)
            audioSource.Play();
    }

    private float GetAverage(Recipe recipe)
    {
        float average = 0f;

        foreach (var stage in recipe.Stages)
            average += ratingsManager.LoadStageRating(stage);

        average /= recipe.Stages.Count * maxRating;

        Debug.Log("Average: " + average);

        return average;
    }
}
