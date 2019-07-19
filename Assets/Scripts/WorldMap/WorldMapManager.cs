using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMapManager : MonoBehaviour
{
    [SerializeField] private GameObject wayPointPanel;

    private List<Waypoint> waypoints = new List<Waypoint>();
    private List<WaypointUI> waypointUI = new List<WaypointUI>();
    private SceneData selectedStage;
    private SceneController sceneController;

    private void Awake()
    {
        sceneController = SingletonManager.GetInstance<SceneController>();

        if (wayPointPanel == null)
            Debug.LogError(gameObject.name + ": No waypoints in list");

        waypoints.AddRange(wayPointPanel.GetComponentsInChildren<Waypoint>());
        waypointUI.AddRange(wayPointPanel.GetComponentsInChildren<WaypointUI>());
    }

    private void OnEnable()
    {
        foreach (var waypointUI in waypointUI)
            waypointUI.OnWaypointArrival.AddListener(SetSelectedStage);
    }

    private void OnDisable()
    {
        foreach (var waypointUI in waypointUI)
            waypointUI.OnWaypointArrival.RemoveListener(SetSelectedStage);
    }

    public void LoadSelectedStage()
    {
        Debug.Log(selectedStage.SceneName);
        sceneController.FadeAndLoadScene(selectedStage.SceneName);
    }

    private void SetSelectedStage(SceneData sceneData)
    {
        Debug.Log("Selected stage: " + sceneData.name);
        selectedStage = sceneData;
    }
}
