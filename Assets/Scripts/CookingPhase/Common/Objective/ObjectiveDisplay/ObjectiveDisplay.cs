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
    [SerializeField] private GameObject objectivePanelPrefab;
    [SerializeField] private Transform objectiveListPanel;

    private List<ObjectiveItemDisplay> objectiveItemDisplayList = new List<ObjectiveItemDisplay>();
    private Button nextButton;
    private CanvasGroup nextButtonCanvasGroup;

    private void Start()
    {
        nextButton = nextButtonPanel.GetComponentInChildren<Button>();
        nextButtonCanvasGroup = nextButtonPanel.GetComponent<CanvasGroup>();

        foreach (var objective in objectiveManager.Objectives)
        {
            // Is this an ObjectiveGroup, i.e. we will need to go through its sub-objectives first
            if (typeof(ObjectiveGroup).IsAssignableFrom(objective.GetType()))
            {
                ObjectiveGroup og = (ObjectiveGroup)objective;
                foreach (var item in og.objectives)
                {
                    ListenToEvents(item);
                }
            }

            // Main objectives
            ListenToEvents(objective);


            // Create a list item to display each objective in the recipe/objective dialog panel
            GameObject o = Instantiate(objectivePanelPrefab, objectiveListPanel, false);
            ObjectiveItemDisplay objItemDisplay = o.GetComponent<ObjectiveItemDisplay>();
            objItemDisplay.CorrespondingObjective = objective;
            objItemDisplay.SetDescriptionText(objective.Description);
            objectiveItemDisplayList.Add(objItemDisplay);
        }
    }

    // Listen to certain objective events
    private void ListenToEvents(Objective objective)
    {
        objective.OnEnd.AddListener(HideNextButton);
        objective.OnEnd.AddListener(ToggleObjectiveItem);
        objective.OnReadyForNext.AddListener(ShowNextButton);
        objective.OnAutomaticallyGoToNext.AddListener(ClickNext);
    }

    private void ClickNext(Objective objective)
    {
        // We simulate clicking the next button to automatically end objectives
        ShowNextButton(objective); // We mimic letting the button slide in to avoid issues with animation...
        nextButtonCanvasGroup.alpha = 0; // ... but hide the button
        nextButton.onClick.Invoke();
    }

    private void ShowNextButton(Objective objective)
    {
        nextButtonCanvasGroup.alpha = 1;
        nextButtonPanel.SetTrigger("SlideIn");
    }

    private void HideNextButton(Objective objective)
    {
        nextButtonPanel.SetTrigger("SlideOut");
    }

    private void ToggleObjectiveItem(Objective objective)
    {
        // TODO: Optimize this?
        objectiveItemDisplayList.Find(x => x.CorrespondingObjective == objective).SetToggleCheckbox(objective.Successful);
    }
}
