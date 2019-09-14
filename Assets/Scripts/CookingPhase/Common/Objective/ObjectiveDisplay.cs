using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectiveDisplay : MonoBehaviour
{
    [SerializeField] private ObjectiveManager objectiveManager;
    [SerializeField] private GameObject objectivePanelPrefab;
    [SerializeField] private Transform objectiveListPanel;

    private List<GameObject> objectivesList = new List<GameObject>();

    private void Start()
    {
        foreach (var objective in objectiveManager.Objectives)
        {
            GameObject obj = Instantiate(objectivePanelPrefab, objectiveListPanel, false);
            obj.GetComponentInChildren<TextMeshProUGUI>().text = objective.Description;
            objectivesList.Add(obj);
        }
    }
}
