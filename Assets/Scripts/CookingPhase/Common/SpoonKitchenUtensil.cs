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

    private void Awake()
    {
        kitchenUtensil = GetComponent<KitchenUtensil>();
        transform.hasChanged = false;
    }

    private void OnEnable()
    {
        MixDuration = 0f;
    }

    private void Update()
    {
        if (!kitchenUtensil.InCookware) { return; }

        if (transform.hasChanged)
        {
            IsMixing = true;
            MixDuration += Time.deltaTime;
            transform.hasChanged = false; // Must always be set back to false
            //Debug.Log(gameObject.name + " is mixing.");
            //Debug.Log(MixDuration);
        }
        else
        {
            IsMixing = false;
        }
    }
}
