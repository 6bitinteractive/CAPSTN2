using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    public StageData StageData;
    public StageResult StageResult;

    private static StageManager stageManager;

    private void Start()
    {
        if (stageManager == null)
            stageManager = SingletonManager.GetInstance<StageManager>();

        // Load results if available
        if (stageManager.StageProgress.TryGetValue(StageData, out StageResult))
        {
            StageResult = stageManager.StageProgress[StageData];
            Debug.Log("Grade: " + StageResult.Grade.ToString() + " | Score: " + StageResult.Score.ToString());
        }
    }
}

public enum Grade
{
    Ungraded, S, A, B, C, D, E, F
}

public struct StageResult
{
    public Grade Grade;
    public int Score;

    public StageResult(Grade grade, int score)
    {
        Grade = grade;
        Score = score;
    }
}
