using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageTracker : MonoBehaviour
{
    public Recipe CurrentRecipe { get; set; }
    public Recipe RecentCompletedRecipe { get; set; }

    private void OnEnable()
    {
        SingletonManager.Register<StageTracker>(this);
    }

    private void OnDisable()
    {
        SingletonManager.UnRegister<StageTracker>();
    }
}
