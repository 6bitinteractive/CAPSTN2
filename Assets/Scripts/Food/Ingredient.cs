using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Food/Ingredient", fileName = "Ingredient")]
public class Ingredient : ScriptableObject
{
    public string DisplayName;
    public Sprite Sprite;
}
