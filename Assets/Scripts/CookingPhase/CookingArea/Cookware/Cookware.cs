using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(DropArea))]

public class Cookware : MonoBehaviour
{
    [HideInInspector] public List<Cookable> CookableIngredients = new List<Cookable>();
    public HeatSetting CurrentHeat { get; set; }

    private DropArea dropArea;

    private void Awake()
    {
        dropArea = GetComponent<DropArea>();
    }

    private void OnEnable()
    {
        dropArea.OnItemDroppedOnArea.AddListener(AddToCookableIngredients);
        dropArea.OnItemDroppedOnArea.AddListener(ShowItemInCookware);
    }

    private void OnDisable()
    {
        dropArea.OnItemDroppedOnArea.RemoveAllListeners();
    }

    private void FixedUpdate()
    {
        if (CookableIngredients.Count > 0)
        {
            foreach (var item in CookableIngredients)
                ApplyHeat(item.Temperature);
        }
    }

    public void ApplyHeat(Slider cookingIngredientTemp)
    {
        if (cookingIngredientTemp == null) { return; }

        // TODO: Apply heat over time
        float heat = 0;
        switch (CurrentHeat)
        {
            case HeatSetting.Low:
                heat = StoveController.AddLowHeat;
                break;
            case HeatSetting.Medium:
                heat = StoveController.AddMediumHeat;
                break;
            case HeatSetting.High:
                heat = StoveController.AddHighHeat;
                break;
            default:
                heat = 0;
                break;
        }

        cookingIngredientTemp.value += heat;
    }

    public void AddToBoilingIngredients(Cookable ingredient)
    {
        CookableIngredients.Add(ingredient);
        Debug.Log("Added to cookware: " + ingredient.gameObject.name);
    }

    private void AddToCookableIngredients(Draggable item)
    {
        if (item == null) { return; }

        Cookable cookableIngredient = item.GetComponent<Cookable>();

        if (cookableIngredient)
        {
            CookableIngredients.Add(cookableIngredient);
        }
    }

    private void ShowItemInCookware(Draggable item)
    {
        if (item == null) { return; }

        Cookable cookableIngredient = item.GetComponent<Cookable>();
        // Show the ingredient in cookware
        cookableIngredient.IngredientInCookware.SetActive(true);
        cookableIngredient.OnIngredientDroppedToCookware.Invoke();

        // Hide the ingredient being dragged to the cookware
        item.gameObject.SetActive(false);
    }
}
