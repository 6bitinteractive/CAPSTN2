using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Food/Recipe", fileName = "Recipe")]

public class Recipe : ScriptableObject
{
    public string DisplayName;
    //[Multiline] public string Description;
    public Location Area;
    //public Ingredient[] Ingredients;
    //public Procedure[] Procedures;
    // TODO: add list of animation states
}

public enum Location
{
    Manila,
    Pampanga
}
