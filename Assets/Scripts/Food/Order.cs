using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Food/Order", fileName = "Order")]
public class Order : ScriptableObject
{
    public string Name;
    public int ScoreValue;
    public Sprite Icon;
}
