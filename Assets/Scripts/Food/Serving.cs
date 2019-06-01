﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Serving : MonoBehaviour
{
    [Header("UI")]
    public Processes ProcessesPanel;

    public Recipe BaseRecipe { get; set; }

    private StageManager stageManager;
    private int currentProcedure;
    private int currentScore;

    private void Start()
    {
        stageManager = SingletonManager.GetInstance<StageManager>();
        BaseRecipe = stageManager.CurrentStage.Recipe;
        Debug.Log("Current recipe: " + BaseRecipe.DisplayName);

        ResetUI();

        // test running a procedure
        if (BaseRecipe.Procedures.Length > 0)
        {
            BaseRecipe.Procedures[currentProcedure].Apply(this);
        }

        // test setting a result
        StageResult result = new StageResult(Grade.C, 50);
        stageManager.StageProgress[stageManager.CurrentStage] = result;

        // test loading result in StageSelection scene
        //Invoke("test", 3f);
    }

    private void test()
    {
        SingletonManager.GetInstance<SceneController>().FadeAndLoadScene("StageSelection");
    }

    public void ResetUI()
    {
        ProcessesPanel.gameObject.SetActive(false);
    }
}
