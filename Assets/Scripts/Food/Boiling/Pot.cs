using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DropArea))]

public class Pot : MonoBehaviour
{
    private DropArea dropArea;

    private void Awake()
    {
        dropArea = GetComponent<DropArea>();
    }

    private void OnEnable()
    {
        dropArea.OnItemDroppedOnArea.AddListener(HideItem);
    }

    private void OnDisable()
    {
        dropArea.OnItemDroppedOnArea.RemoveListener(HideItem);
    }

    private void HideItem(DraggableItem item)
    {
        if (item != null)
            item.gameObject.SetActive(false);
    }
}
