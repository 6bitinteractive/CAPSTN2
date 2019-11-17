using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CookingOverviewManager : MonoBehaviour
{
    [SerializeField] private Recipe recipe;
    [SerializeField] private GameObject stepsPanel;
    [SerializeField] private Image recipeName;
    [SerializeField] private float delayIconEnter = 0.1f;
    [SerializeField] private float delayIconFlip = 0.5f;
    [Range(1, 3)] [SerializeField] private int MinRating = 2;

    public UnityEvent OnAllStagesDone = new UnityEvent();

    private List<Step> steps = new List<Step>();
    private List<StepUI> stepsUI = new List<StepUI>();
    private List<Animator> stepUIAnimators = new List<Animator>();
    private SceneData selectedStage;
    private SceneController sceneController;
    private RatingsManager ratingsManager;
    private StageTracker stageTracker;

    private void Awake()
    {
        if (stepsPanel == null)
            Debug.LogError(gameObject.name + ": No steps in list");

        steps.AddRange(stepsPanel.GetComponentsInChildren<Step>());
        stepsUI.AddRange(stepsPanel.GetComponentsInChildren<StepUI>());

        foreach (var step in steps)
            stepUIAnimators.Add(step.GetComponent<Animator>());
    }

    private void OnEnable()
    {
        foreach (var stepUI in stepsUI)
            stepUI.OnStageSelect.AddListener(SetSelectedStage);
    }

    private void OnDisable()
    {
        foreach (var stepUI in stepsUI)
            stepUI.OnStageSelect.RemoveListener(SetSelectedStage);
    }

    private void Start()
    {
        sceneController = SingletonManager.GetInstance<SceneController>();
        ratingsManager = SingletonManager.GetInstance<RatingsManager>();
        stageTracker = SingletonManager.GetInstance<StageTracker>();

        // Set this as the game's current recipe
        stageTracker.CurrentRecipe = recipe;

        recipeName.sprite = recipe.DishNameImage;

        UnlockSteps();
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
            // Or, in the case of the last stage, simply return true (i.e. the only requirement is that it is unlocked itself
            steps[i].Current = !steps[i].Locked && (i + 1 >= steps.Count ? true : steps[i + 1].Locked);

            // Are all the steps done
            if (i == steps.Count - 1) // Is this the last step
            {
                if (steps[i].Rating >= MinRating) // Is it above the required minimum rating
                {
                    steps[i].Current = false; // Don't set the last stage as the current stage (i.e., will not make it's icon bigger)
                    stageTracker.RecentCompletedRecipe = recipe; // Set this as the recently completed recipe
                    OnAllStagesDone.Invoke();
                }
            }
        }

        // Animate icons
        StartCoroutine(AnimateIcons());
    }

    public void HideIcons()
    {
        recipeName.gameObject.SetActive(false);
        stepsPanel.SetActive(false);
    }

    private IEnumerator AnimateIcons()
    {
        // Update StepUI
        for (int i = 0; i < stepsUI.Count; i++)
        {
            stepsUI[i].UpdateStepUI();
        }

        foreach (var stepAnimator in stepUIAnimators)
        {
            stepAnimator.SetTrigger("Enter");
            yield return new WaitForSeconds(delayIconEnter);
        }

        yield return new WaitForSeconds(delayIconFlip);

        // Flip the current stage
        for (int i = 0; i < steps.Count; i++)
        {
            if (steps[i].Current)
            {
                stepsUI[i].SetAsCurrent();
                steps[i].GetComponent<Animator>().SetTrigger("Flip");
                yield break;
            }
        }

    }
}
