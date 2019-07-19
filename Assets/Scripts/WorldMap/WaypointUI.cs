using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

[RequireComponent(typeof(Waypoint))]

[System.Serializable] public class OnWayPointArrival : UnityEvent<SceneData> { }

public class WaypointUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI waypointNameText;

    [HideInInspector] public OnWayPointArrival OnWaypointArrival = new OnWayPointArrival();

    private Waypoint waypoint;

    private void Start()
    {
        waypoint = GetComponent<Waypoint>();

        if (waypoint.Locked)
            gameObject.SetActive(false);

            waypointNameText.text = waypoint.DisplayName;
    }

    public void InvokeOnWaypointArrival()
    {
        OnWaypointArrival.Invoke(waypoint.StageScene);
    }
}
