using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StepRatingUI : MonoBehaviour
{
    private TextMeshProUGUI ratingText;

    private void Awake()
    {
        ratingText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void ShowRating(StepRating stepRating)
    {
        gameObject.SetActive(true); // Enable first to avoid null ref errors
        ratingText.text = stepRating.Rating.ToString();
    }
}
