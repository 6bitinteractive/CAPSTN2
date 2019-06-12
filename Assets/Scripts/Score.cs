using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    // Place holder script to display score in dining phase
    public TextMeshProUGUI ScoreText;
    public float TotalScore;

    void Update()
    {
        DisplayScore();
    }

    public void EarnScore(float scoreValue)
    {
        TotalScore += scoreValue;
    }

    public void DisplayScore()
    {
        ScoreText.text = TotalScore.ToString();
    }
}
