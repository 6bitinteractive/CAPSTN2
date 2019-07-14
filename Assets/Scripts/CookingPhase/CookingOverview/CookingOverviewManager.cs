using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingOverviewManager : MonoBehaviour
{
    [SerializeField] private GameObject stepsPanel;

    private List<Step> steps = new List<Step>();
    private List<StepUI> stepsUI = new List<StepUI>();
    private SceneData selectedStage;
    private SceneController sceneController;
    private RatingsManager ratingsManager;

    private void Awake()
    {
        sceneController = SingletonManager.GetInstance<SceneController>();
        ratingsManager = SingletonManager.GetInstance<RatingsManager>();

        if (stepsPanel == null)
            Debug.LogError(gameObject.name + ": No steps in list");

        steps.AddRange(stepsPanel.GetComponentsInChildren<Step>());
        stepsUI.AddRange(stepsPanel.GetComponentsInChildren<StepUI>());
    }

    private void OnEnable()
    {
        foreach (var stepUI in stepsUI)
            stepUI.OnStageSelect.AddListener(SetSelectedStage);

        UnlockSteps();
    }

    private void OnDisable()
    {
        foreach (var stepUI in stepsUI)
            stepUI.OnStageSelect.RemoveListener(SetSelectedStage);
    }

    public void LoadSelectedStage()
    {
        sceneController.FadeAndLoadScene(selectedStage.SceneName);
    }

    private void SetSelectedStage(SceneData sceneData)
    {
        Debug.Log("Selected stage: " + sceneData.name);
        selectedStage = sceneData;
    }

    private void UnlockSteps()
    {
        // Check if steps should be unlocked
        foreach (var step in steps)
        {
            step.Rating = ratingsManager.LoadStageRating(step.StageScene); // Store the rating
            step.Locked = step.Rating <= 0; // Unlock if there's a stored rating, i.e. not 0; Make sure stages with no ratings remain locked
        }

        // Always unlock the first step
        steps[0].Locked = false;
    }
}
