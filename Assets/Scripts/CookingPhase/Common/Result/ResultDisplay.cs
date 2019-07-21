using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultDisplay : MonoBehaviour
{
    [SerializeField] private ResultText resultText;
    [SerializeField] private GameObject starRatingsPanel;

    private List<StarRating> starRatings = new List<StarRating>();

    private void Awake()
    {
        starRatings.AddRange(starRatingsPanel.GetComponentsInChildren<StarRating>());
    }

    public void DisplayResult(int rating)
    {
        for (int i = 0; i < rating; i++)
            starRatings[i].SwitchOn();

        resultText.DisplayTextRating(rating);
    }
}
