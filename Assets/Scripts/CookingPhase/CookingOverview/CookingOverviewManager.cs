using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CookingOverviewManager : MonoBehaviour
{
    [SerializeField] private GameObject stepsPanel;
    [Range(1, 3)] [SerializeField] private int MinRating = 2;

    public UnityEvent OnAllStagesDone = new UnityEvent();

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
        for (int i = 0; i < steps.Count; i++)
        {
            steps[i].Rating = ratingsManager.LoadStageRating(steps[i].StageScene); // Load the rating
            steps[i].Locked = steps[i].Rating <= 0; // Unlock if there's a stored rating, i.e. not 0; Make sure stages with no ratings remain locked
            steps[i].Locked = steps[i > 0 ? i - 1 : 0].Rating < MinRating; // Remain locked if previous stage wasn't successfully beaten
        }

        // Always unlock the first step
        steps[0].Locked = false;

        for (int i = 0; i < steps.Count; i++)
        {
            // It's the most recent unlocked step if the step itself is unlocked and its right neighbor is locked
            steps[i].Current = !steps[i].Locked && steps[i + 1 >= steps.Count ? i : i + 1].Locked;

            // Are all the steps done
            if (i == steps.Count - 1) // Is this the last step
            {
                if (steps[i].Rating >= MinRating) // Is it above the required minimum rating
                    OnAllStagesDone.Invoke();
            }
        }
    }
}
