using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(Step))]

public class StepUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI stepNameText;
    [SerializeField] private GameObject[] starRatingImages;

    private Step step;

    private void Start() // Do this at start to make sure the step object has loaded its data (eg. rating)
    {
        step = GetComponent<Step>();

        // [Debug] Display the name of the step
        stepNameText.text = step.DisplayName;

        // If there's a rating for the step, show same number of stars
        if (step.Rating > 0)
        {
            for (int i = 0; i < step.Rating; i++)
                starRatingImages[i].SetActive(true);
        }
    }
}
