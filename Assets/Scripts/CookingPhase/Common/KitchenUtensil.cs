using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Draggable))]

public class KitchenUtensil : MonoBehaviour
{
    public bool InCookware { get; private set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Cookware cookware = collision.GetComponent<Cookware>();

        if (!cookware) { return; }

        Debug.Log(gameObject.name + " is in the cookware");
        InCookware = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log(gameObject.name + " is away from the cookware");
        InCookware = false;
    }
}
