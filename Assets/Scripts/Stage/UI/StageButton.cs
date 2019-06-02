﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

[RequireComponent(typeof(Stage))]
public class StageButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI stageButtonText;
    private StageData stageData;

    private static StageManager stageManager;

    private void Start()
    {
        if (stageManager == null)
            stageManager = SingletonManager.GetInstance<StageManager>();

        stageData = GetComponent<Stage>().StageData;
        stageButtonText.text = stageData.DisplayName;
    }

    public void SelectAsCurrentStage()
    {
        stageManager.CurrentStage = stageData;
    }
}
