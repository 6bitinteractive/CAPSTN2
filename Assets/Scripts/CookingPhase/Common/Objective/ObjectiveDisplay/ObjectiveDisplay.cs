using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class ObjectiveDisplay : MonoBehaviour
{
    [SerializeField] private ObjectiveManager objectiveManager;
    [SerializeField] private Animator nextButtonPanel;
    [SerializeField] private AudioSource nextButtonAudioSource;
    [SerializeField] private AudioClip nextButtonClickSfx;
    [SerializeField] private GameObject objectivePanelPrefab;
    [SerializeField] private Transform objectiveListPanel;

    private List<ObjectiveItemUI> objectiveItemDisplayList = new List<ObjectiveItemUI>();
    private Button nextButton;
    private CanvasGroup nextButtonCanvasGroup;
    private Coroutine nextButtonCoroutine;

    private void Start()
    {
        nextButton = nextButtonPanel.GetComponentInChildren<Button>();
        nextButtonCanvasGroup = nextButtonPanel.GetComponent<CanvasGroup>();

        objectiveManager.OnAllObjectivesDone.AddListener(() => StartCoroutine(HideObjectives()));

        for (int i = 0; i < objectiveManager.Objectives.Count; i++)
        {
            // Listen to certain objective events
            objectiveManager.Objectives[i].OnEnd.AddListener(HideNextButton);
            objectiveManager.Objectives[i].OnEnd.AddListener(ToggleObjectiveItem);
            objectiveManager.Objectives[i].OnReadyForNext.AddListener(ShowNextButton);
            objectiveManager.Objectives[i].OnAutomaticallyGoToNext.AddListener(ClickNext);

            // Create a list item to display each objective in the recipe/objective dialog panel
            GameObject o = Instantiate(objectivePanelPrefab, objectiveListPanel, false);
            ObjectiveItemUI objItemDisplay = o.GetComponent<ObjectiveItemUI>();
            objItemDisplay.CorrespondingObjective = objectiveManager.Objectives[i];
            objItemDisplay.SetData(string.Format("Step {0} of {1}", i + 1, objectiveManager.Objectives.Count), objectiveManager.Objectives[i].Description);
            objItemDisplay.transform.SetAsFirstSibling(); // The latest item is at the back
            objectiveItemDisplayList.Add(objItemDisplay);
        }
    }

    private void ClickNext(Objective objective)
    {
        // Remove the next button's click sfx
        nextButtonAudioSource.clip = null;

        // We simulate clicking the next button to automatically end objectives
        ShowNextButton(objective); // We mimic letting the button slide in to avoid issues with animation...
        nextButtonCanvasGroup.alpha = 0; // ... but hide the button
        nextButton.onClick.Invoke();
        Debug.Log("Automatically clicked Next button.");

        nextButtonAudioSource.clip = nextButtonClickSfx;
    }

    private void ShowNextButton(Objective objective)
    {
        nextButtonCanvasGroup.alpha = 1;
        //nextButton.interactable = true;
        if (nextButtonCoroutine != null)
            StopCoroutine(nextButtonCoroutine);
        nextButtonCoroutine = StartCoroutine(AnimateNextButton(true));
        Debug.Log("Showing Next button.");
    }

    private void HideNextButton(Objective objective)
    {
        if (nextButtonCoroutine != null)
            StopCoroutine(nextButtonCoroutine);
        nextButtonCoroutine = StartCoroutine(AnimateNextButton(false));
        Debug.Log("Hid Next button");
    }

    private void ToggleObjectiveItem(Objective objective)
    {
        // TODO: Optimize this?
        ObjectiveItemUI obj = objectiveItemDisplayList.Find(x => x.CorrespondingObjective == objective);
        obj.SetCheckmark(objective.Successful);
        obj.Show(false);
    }

    private IEnumerator HideObjectives()
    {
        yield return new WaitForSeconds(2f);
        objectiveListPanel.gameObject.SetActive(false);
    }

    private IEnumerator AnimateNextButton(bool slideIn)
    {
        nextButton.interactable = false;
        string state = "";
        do
        {
            state = slideIn ? "SlideInOnTrigger" : "SlideOutOnTrigger";
            nextButtonPanel.ResetTrigger("SlideIn");
            nextButtonPanel.ResetTrigger("SlideOut");
            nextButtonPanel.SetTrigger(slideIn ? "SlideIn" : "SlideOut");
            yield return null;
        } while (!AnimatorUtils.IsInState(nextButtonPanel, state));

        yield return new WaitUntil(() => AnimatorUtils.IsDonePlaying(nextButtonPanel, state));
        nextButton.interactable = true;
    }
}
