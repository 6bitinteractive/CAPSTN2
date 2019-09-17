using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ObjectiveDisplay : MonoBehaviour
{
    [SerializeField] private ObjectiveManager objectiveManager;
    [SerializeField] private GameObject objectivePanelPrefab;
    [SerializeField] private Transform objectiveListPanel;
    private List<ObjectiveItemDisplay> objectiveItemDisplayList = new List<ObjectiveItemDisplay>();

    private void Start()
    {
        foreach (var objective in objectiveManager.Objectives)
        {
            // Listen to when objective ends
            objective.OnEnd.AddListener(ToggleObjectiveItem);

            // Create a list item to display each objective in the recipe/objective dialog panel
            GameObject o = Instantiate(objectivePanelPrefab, objectiveListPanel, false);
            ObjectiveItemDisplay objItemDisplay = o.GetComponent<ObjectiveItemDisplay>();
            objItemDisplay.CorrespondingObjective = objective;
            objItemDisplay.SetDescriptionText(objective.Description);
            objectiveItemDisplayList.Add(objItemDisplay);
        }
    }

    private void ToggleObjectiveItem(Objective objective)
    {
        // TODO: Optimize this?
        objectiveItemDisplayList.Find(x => x.CorrespondingObjective == objective).SetToggleCheckbox(objective.Successful);
    }
}
