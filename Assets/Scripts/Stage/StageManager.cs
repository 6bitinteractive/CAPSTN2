using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    //[SerializeField] private StageData[] stages;

    public Dictionary<StageData, StageResult> StageProgress = new Dictionary<StageData, StageResult>();
    public StageData CurrentStage { get; set; }

    public void Awake()
    {
        SingletonManager.Register<StageManager>(this);
    }
}

