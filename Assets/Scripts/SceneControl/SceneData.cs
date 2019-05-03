using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Kept in a scriptable object to easily switch scenes and avoid retyping scene names.
// This also avoids having to hunt down for scripts to change in case multiple scenes need to point to a new scene.

[CreateAssetMenu(menuName = "Data/Scene Data", fileName = "Type-Scene")]
public class SceneData : ScriptableObject
{
    public string SceneName;
}
