using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Step : MonoBehaviour
{
    public SceneData StageScene;
    public int Rating { get; set; }
    public bool Locked { get; set; }
    public bool Current { get; set; } // The most recently unlocked step
}
