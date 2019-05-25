using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Serving : MonoBehaviour
{
    public Recipe BaseRecipe;
    public Quality FoodQuality { get; set; }

    private int currentProcedure;

    private void Start()
    {
        BaseRecipe.Procedures[currentProcedure].Apply(this); // test
    }

    public enum Quality
    {
        Perfect,
        Normal,
        Crude
    }
}
