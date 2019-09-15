using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kitchen : MonoBehaviour
{
    [Tooltip("The Recipe related to this scene.")]
    public Recipe Recipe;

    [Tooltip("The SceneData that loads this scene.")]
    public SceneData StageScene;

    [Tooltip("The cookware in this scene. Eg. pot, pan, etc.")]
    public Cookware Cookware;
}
