using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Serving : MonoBehaviour
{
    public GameEvent OnRecipeDoneEvent;
    public Recipe BaseRecipe { get; set; }
    public Quality FoodQuality { get; set; }

    private int currentProcedure;

    private void Start()
    {
        BaseRecipe = SingletonManager.GetInstance<StageManager>().CurrentStage.Recipe;
        BaseRecipe.Procedures[currentProcedure].Apply(this); // test

        // broadcast score
        //OnRecipeDoneEvent.sentInt = 100;
        //OnRecipeDoneEvent.Raise();
    }

    public enum Quality
    {
        Perfect,
        Normal,
        Crude
    }
}
