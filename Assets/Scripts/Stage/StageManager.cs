using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public StageData CurrentStage { get; set; }

    private Dictionary<StageData, StageData.Grade> StageProgress;

    public void Awake()
    {
        SingletonManager.Register<StageManager>(this);
    }

    public void UpdateGrade(StageData stage, int score)
    {
        // calculate grade
        //StageProgress[stage] = grade;
    }
}
