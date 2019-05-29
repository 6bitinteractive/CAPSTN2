using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Stage", fileName = "Stage00")]
public class Stage : ScriptableObject
{
    public string DisplayName;
    public Location Area;
    public Recipe Recipe;

    public enum Location
    {
        Manila
    }

    public enum Grade
    {
        S, A, B, C, D, E, F
    }
}
