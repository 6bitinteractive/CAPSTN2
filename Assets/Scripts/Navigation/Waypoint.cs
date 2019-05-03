using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    private const string tagName = "Waypoint"; // Make sure the tag has been declared in the Tags and Layers project setting

    public List<Waypoint> Neighbors;
    public Waypoint Previous { get; set; }
    public float Distance { get; set; }

    private void Start()
    {
        gameObject.tag = tagName; // Do not set tags in Awake() or OnValidate()
    }

    // This will draw a line to all of the neighbors of this waypoint in the Scene view of Unity
    // `OnDrawGizmos()` to only draw neighbors of the currently selected waypoint
    private void OnDrawGizmos()
    {
        if (Neighbors == null)
            return;

        Gizmos.color = new Color(0.0f, 0.0f, 0.0f);
        foreach (var neighbor in Neighbors)
        {
            if (neighbor != null)
                Gizmos.DrawLine(transform.position, neighbor.transform.position);
        }
    }
}

// Reference: Waypoint Pathing System
// https://www.trickyfast.com/2017/09/21/building-a-waypoint-pathing-system-in-unity/
// GameObject.tag
// https://docs.unity3d.com/ScriptReference/GameObject-tag.html
