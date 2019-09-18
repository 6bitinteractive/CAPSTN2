using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SceneLoader))]

public class ResultDisplay : MonoBehaviour
{
    [SerializeField] private ResultText resultText;
    [SerializeField] private GameObject starRatingsPanel;
    [SerializeField] private Kitchen kitchen;

    private List<StarRating> starRatings = new List<StarRating>();
    private SceneLoader sceneLoader;

    private void Start()
    {
        starRatings.AddRange(starRatingsPanel.GetComponentsInChildren<StarRating>());
        sceneLoader = GetComponent<SceneLoader>();
    }

    public void DisplayResult(int rating = 3)
    {
        for (int i = 0; i < rating; i++)
            starRatings[i].SwitchOn();

        resultText.DisplayTextRating(rating);
    }

    public void ReplayScene()
    {
        sceneLoader.LoadScene(kitchen.StageScene);
    }

    public void LoadNextScene()
    {
        // Load the next scene except when it's the last stage
        //int nextScene = step.Recipe.Stages.IndexOf(step.StageScene) + 1;
        //sceneLoader.LoadScene(nextScene >= step.Recipe.Stages.Count ? step.Recipe.CookingOverview : step.Recipe.Stages[nextScene]);

        // For now, the Next button always loads the CookingOverview scene
        sceneLoader.LoadScene(kitchen.Recipe.CookingOverview);
    }
}
