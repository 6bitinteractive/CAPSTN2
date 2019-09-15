using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(DropArea))]

public class Cookware : MonoBehaviour
{
    [SerializeField] private RectTransform lidPosition;
    [HideInInspector] public List<Cookable> CookableIngredients = new List<Cookable>();

    public RectTransform LidPosition => lidPosition;
    //public HeatSetting CurrentHeat { get; set; }

    private DropArea dropArea;

    private void Awake()
    {
        dropArea = GetComponent<DropArea>();
    }

    private void OnEnable()
    {
        dropArea.OnItemDroppedOnArea.AddListener(AddToCookableIngredients);
        dropArea.OnItemDroppedOnArea.AddListener(ShowIngredientInCookware);
    }

    private void OnDisable()
    {
        dropArea.OnItemDroppedOnArea.RemoveAllListeners();
    }

    //private void FixedUpdate()
    //{
    //    if (CookableIngredients.Count > 0)
    //    {
    //        foreach (var item in CookableIngredients)
    //            ApplyHeat(item.Temperature);
    //    }
    //}

    //public void ApplyHeat(Slider cookingIngredientTemp)
    //{
    //    if (cookingIngredientTemp == null) { return; }

    //    // TODO: Apply heat over time
    //    float heat = 0;
    //    switch (CurrentHeat)
    //    {
    //        case HeatSetting.Low:
    //            heat = StoveController.LowHeatValue;
    //            break;
    //        case HeatSetting.Medium:
    //            heat = StoveController.MediumHeatValue;
    //            break;
    //        case HeatSetting.High:
    //            heat = StoveController.HighHeatValue;
    //            break;
    //        default:
    //            heat = 0;
    //            break;
    //    }

    //    cookingIngredientTemp.value += heat;
    //}

    //public void AddToBoilingIngredients(Cookable ingredient)
    //{
    //    CookableIngredients.Add(ingredient);
    //    Debug.Log("Added to cookware: " + ingredient.gameObject.name);
    //}

    private void AddToCookableIngredients(Draggable item)
    {
        if (item == null) { return; }

        Cookable cookableIngredient = item.GetComponent<Cookable>();

        if (cookableIngredient)
        {
            CookableIngredients.Add(cookableIngredient);
        }
    }

    private void ShowIngredientInCookware(Draggable item)
    {
        Cookable cookable = item.GetComponent<Cookable>();
        if (cookable == null) { return; }
        cookable.ShowIngredientInCookware();
    }
}
