using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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

    [Header("Events")]
    public UnityEvent OnReady = new UnityEvent();

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
            for (int i = 0; i < dishImages.Count; i++)
            {
                dishImages[i].sprite = null;
                dishImages[i].sprite = recipe.FailedFinalDishSequence[i];
                audioSource.clip = failSfx;
            }
        }
        else // Success
        {
            for (int i = 0; i < dishImages.Count; i++)
            {
                dishImages[i].sprite = recipe.SuccessfulFinalDishSequence[i];
                audioSource.clip = successSfx;
            }
        }

        // Set dish name
        dishName.sprite = recipe.DishNameImage;

        // Play audio
        if (audioSource.clip != null)
            audioSource.Play();

        OnReady.Invoke();
    }

    private float GetAverage(Recipe recipe)
    {
        float average = 0f;

        foreach (var stage in recipe.Stages)
            average += ratingsManager.LoadStageRating(stage);

        average /= recipe.Stages.Count;

        Debug.Log("Average: " + average);

        return average;
    }
}

// Really weird bug: image is not displayed when setting the sprites in scripts
// Fix: set the image to null first, and/or set the image type to "Filled"---I had to use both fixes
// Reference: https://stackoverflow.com/a/57228049
