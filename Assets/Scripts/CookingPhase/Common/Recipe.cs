using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Recipe Scene List", fileName = "Recipe00")]

public class Recipe : ScriptableObject
{
    public SceneData CookingOverview;
    public List<SceneData> Stages;
}
