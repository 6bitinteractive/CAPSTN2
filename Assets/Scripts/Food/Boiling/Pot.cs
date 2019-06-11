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
        dropArea.OnItemDroppedOnArea.AddListener(ShowItemInPot);
    }

    private void OnDisable()
    {
        dropArea.OnItemDroppedOnArea.RemoveListener(ShowItemInPot);
    }

    private void ShowItemInPot(DraggableItem item)
    {
        if (item != null)
        {
            item.GetComponent<PotIngredient>().IngredientInPot.SetActive(true);
            item.gameObject.SetActive(false);
        }
    }
}
