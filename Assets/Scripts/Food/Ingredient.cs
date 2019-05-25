using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Food/Create Ingredient", fileName = "Ingredient00")]
public class Ingredient : ScriptableObject
{
    public string DisplayName;
    public Sprite Sprite;
}
