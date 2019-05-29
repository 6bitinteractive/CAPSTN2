using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Food/Recipe", fileName = "Recipe00")]
public class Recipe : ScriptableObject
{
    [SerializeField] private Ingredient[] ingredients;
    public Procedure[] Procedures;
    public string DisplayName;
    // TODO: add list of animation states
}
