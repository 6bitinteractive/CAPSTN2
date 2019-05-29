using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public Stage CurrentStage { get { return stages[currentStageIndex]; } }

    [SerializeField] private Stage[] stages;
    private Dictionary<Stage, Stage.Grade> StageProgress;
    private int currentStageIndex;

    public void Awake()
    {
        SingletonManager.Register<StageManager>(this);
    }

    public void UpdateGrade(Stage stage, int score)
    {
        // calculate grade
        //StageProgress[stage] = grade;
    }

    public void MoveToNextStage()
    {
        currentStageIndex++;
    }
}
