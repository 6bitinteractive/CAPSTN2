using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]

public class DropArea : MonoBehaviour
{
    public DraggableItem Item { get; private set; }
    [HideInInspector] public OnItemDrop OnItemDroppedOnArea = new OnItemDrop();

    private void OnTriggerStay2D(Collider2D collision)
    {
        Item = collision.GetComponent<DraggableItem>();

        if (!Item.Grabbed)
            OnItemDroppedOnArea.Invoke(Item);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Item = null;
    }
}
