using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PolygonCollider2D))]

public class DropArea : MonoBehaviour
{
    [HideInInspector] public OnItemDrop OnItemDroppedOnArea = new OnItemDrop();

    private Draggable item;
    private Rigidbody2D rb;
    private PolygonCollider2D polygonCollider;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        polygonCollider = GetComponent<PolygonCollider2D>();

        rb.gravityScale = 0f;
        polygonCollider.isTrigger = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        item = collision.GetComponent<Draggable>();
        if (item == null) { return; }

        if (!item.Grabbed)
        {
            Debug.Log("Item dropped in " + gameObject.name + ": " + item.name);
            OnItemDroppedOnArea.Invoke(item);
            item = null;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        item = null;
    }
}
