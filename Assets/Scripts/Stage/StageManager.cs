using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    //[SerializeField] private StageData[] stages;

    public StageData CurrentStage { get; set; }
    public Dictionary<StageData, StageResult> StageProgress = new Dictionary<StageData, StageResult>();

    public void Awake()
    {
        SingletonManager.Register<StageManager>(this);
    }
}

