using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(KitchenUtensil))]
[RequireComponent(typeof(Draggable))]
[RequireComponent(typeof(PolygonCollider2D))]

public class SpoonKitchenUtensil : MonoBehaviour
{
    public bool IsMixing { get; private set; }
    public float MixDuration { get; set; }

    private KitchenUtensil kitchenUtensil;
    private Draggable draggable;
    private PolygonCollider2D polygonCollider2D;

    private void Awake()
    {
        kitchenUtensil = GetComponent<KitchenUtensil>();
        draggable = GetComponent<Draggable>();
        polygonCollider2D = GetComponent<PolygonCollider2D>();
        polygonCollider2D.isTrigger = true; // This is for checking if it's within a cookware's collider
        transform.hasChanged = false;
    }

    private void Update()
    {
        if (!kitchenUtensil.InCookware) { return; }

        if (transform.hasChanged)
        {
            Debug.Log(gameObject.name + " is mixing.");
            IsMixing = true;
            MixDuration += Time.deltaTime;
            Debug.Log(MixDuration);
            transform.hasChanged = false; // Must always be set back to false
        }
        else
        {
            IsMixing = false;
        }
    }
}
