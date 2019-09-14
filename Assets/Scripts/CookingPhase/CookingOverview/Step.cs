using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Step : MonoBehaviour
{
    [Tooltip("The Recipe related to this scene.")]
    public Recipe Recipe;

    [Tooltip("The SceneData that loads this scene.")]
    public SceneData StageScene;

    public int Rating { get; set; }
    public bool Locked { get; set; }
    public bool Current { get; set; } // The most recently unlocked step
}
