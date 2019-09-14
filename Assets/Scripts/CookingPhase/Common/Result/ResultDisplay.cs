using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultDisplay : MonoBehaviour
{
    [SerializeField] private ResultText resultText;
    [SerializeField] private GameObject starRatingsPanel;
    [SerializeField] private Step step;
    [SerializeField] private SceneLoader sceneLoader;

    private List<StarRating> starRatings = new List<StarRating>();

    private void Awake()
    {
        starRatings.AddRange(starRatingsPanel.GetComponentsInChildren<StarRating>());
    }

    public void DisplayResult(int rating = 3)
    {
        for (int i = 0; i < rating; i++)
            starRatings[i].SwitchOn();

        resultText.DisplayTextRating(rating);
    }

    public void ReplayScene()
    {
        sceneLoader.LoadScene(step.StageScene);
    }

    public void LoadNextScene()
    {
        // Load the next scene except when it's the last stage
        //int nextScene = step.Recipe.Stages.IndexOf(step.StageScene) + 1;
        //sceneLoader.LoadScene(nextScene >= step.Recipe.Stages.Count ? step.Recipe.CookingOverview : step.Recipe.Stages[nextScene]);

        // For now, the Next button always loads the CookingOverview scene
        sceneLoader.LoadScene(step.Recipe.CookingOverview);
    }
}
