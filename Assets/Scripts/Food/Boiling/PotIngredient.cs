using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(DraggableItem))]

public class PotIngredient : MonoBehaviour
{
    [SerializeField] private GameObject ingredientInPot;

    private DraggableItem ingredient;

    private void Awake()
    {
        ingredient = GetComponent<DraggableItem>();
    }

    private void OnEnable()
    {
        ingredient.OnDropItem.AddListener(ShowIngredientInPot);
    }

    private void OnDisable()
    {
        ingredient.OnDropItem.RemoveListener(ShowIngredientInPot);
    }

    private void ShowIngredientInPot(DraggableItem item)
    {
        ingredientInPot.SetActive(true);
    }
}
