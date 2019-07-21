using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    private const string tagName = "Waypoint"; // Make sure the tag has been declared in the Tags and Layers project setting
    public Sprite GlowSprite;
    public string DisplayName;
    public SceneData StageScene;
    public bool Locked { get; set; }
    public List<Waypoint> Neighbors;
    public Waypoint Previous { get; set; }
    public float Distance { get; set; }
    private Sprite currentSprite;

    private void Start()
    {
        gameObject.tag = tagName; // Do not set tags in Awake() or OnValidate()
        currentSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
     
        if (collision.gameObject.layer == 8)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = GlowSprite;
        }
      
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = currentSprite;
        }
    }
}

// Reference: Waypoint Pathing System
// https://www.trickyfast.com/2017/09/21/building-a-waypoint-pathing-system-in-unity/
// GameObject.tag
// https://docs.unity3d.com/ScriptReference/GameObject-tag.html
