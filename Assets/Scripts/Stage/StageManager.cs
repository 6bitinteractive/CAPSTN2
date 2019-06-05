using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    [SerializeField] private StageData[] stages;

    [Tooltip("Can player choose the stage to play from the stage selection scene?")]
    [SerializeField] private bool stageSelectionAvailable;

    public Dictionary<StageData, StageResult> StageProgress = new Dictionary<StageData, StageResult>();
    public StageData CurrentStage { get; set; }

    private int nextPlayableStage;

    public void Awake()
    {
        SingletonManager.Register<StageManager>(this);

        if (!stageSelectionAvailable)
        {
            CurrentStage = stages[nextPlayableStage];
        }
    }

    public void MoveToNextStage()
    {
        nextPlayableStage++;

        if (nextPlayableStage < stages.Length)
            CurrentStage = stages[nextPlayableStage];
        else
            Debug.Log("Finished all stages.");

        Debug.Log("Next Stage: " + CurrentStage.DisplayName);
    }
}

