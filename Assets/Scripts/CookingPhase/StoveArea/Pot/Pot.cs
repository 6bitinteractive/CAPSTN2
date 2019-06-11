using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(DropArea))]

public class Pot : MonoBehaviour
{
    [SerializeField] private StoveControllerUI stoveController;
    [HideInInspector] public List<Boilable> BoilingIngredients = new List<Boilable>();

    private DropArea dropArea;

    private void Awake()
    {
        dropArea = GetComponent<DropArea>();
    }

    private void OnEnable()
    {
        dropArea.OnItemDroppedOnArea.AddListener(AddToBoilingIngredients);
        dropArea.OnItemDroppedOnArea.AddListener(ShowItemInPot);
    }

    private void OnDisable()
    {
        dropArea.OnItemDroppedOnArea.RemoveListener(AddToBoilingIngredients);
        dropArea.OnItemDroppedOnArea.RemoveListener(ShowItemInPot);
    }

    private void FixedUpdate()
    {
        if (BoilingIngredients.Count > 0)
        {
            foreach (var item in BoilingIngredients)
                ApplyHeat(item.Temperature);
        }
    }

    public void AddToBoilingIngredients(Boilable ingredient)
    {
        BoilingIngredients.Add(ingredient);
        Debug.Log("Added to pot: " + ingredient.gameObject.name);
    }

    public void ApplyHeat(Slider boilingIngredientTemp)
    {
        // TODO: Apply heat over time
        float heat = 0;
        switch (stoveController.CurrentTemperatureSetting)
        {
            case TemperatureSetting.Low:
                heat = stoveController.AddLowHeat;
                break;
            case TemperatureSetting.Medium:
                heat = stoveController.AddMediumHeat;
                break;
            case TemperatureSetting.High:
                heat = stoveController.AddHighHeat;
                break;
            default:
                heat = 0;
                break;
        }

        boilingIngredientTemp.value += heat;

        Debug.Log("Boiling: " + boilingIngredientTemp.gameObject.name);
    }

    private void AddToBoilingIngredients(DraggableItem item)
    {
        Boilable boilingIngredient = item.GetComponent<PotIngredient>().IngredientInPot.GetComponent<Boilable>();

        if (boilingIngredient)
        {
            BoilingIngredients.Add(boilingIngredient);
        }
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
