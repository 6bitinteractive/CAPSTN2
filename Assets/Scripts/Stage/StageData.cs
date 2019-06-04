using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Stage", fileName = "Stage")]
public class StageData : ScriptableObject
{
    public string DisplayName;
    public Recipe Recipe;
}
