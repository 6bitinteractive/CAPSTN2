using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

[RequireComponent(typeof(Step))]
[RequireComponent(typeof(Button))]

[System.Serializable] public class OnStageSelect : UnityEvent<SceneData> { }

public class StepUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI stepNameText;
    [SerializeField] private GameObject[] starRatingImages;

    [HideInInspector] public OnStageSelect OnStageSelect = new OnStageSelect();

    private Step step;
    private Button button;

    private void Start() // Do this at Start (instead of Awake) to make sure the step object has loaded its data (eg. rating)
    {
        step = GetComponent<Step>();

        button = GetComponent<Button>();

        if (step.Locked)
            button.interactable = false;

        button.onClick.AddListener(InvokeOnStageSelect);

        // [Debug] Display the name of the step
        stepNameText.text = step.DisplayName;

        // If there's a rating for the step, show same number of stars
        if (step.Rating > 0)
        {
            for (int i = 0; i < step.Rating; i++)
                starRatingImages[i].SetActive(true);
        }
    }

    private void InvokeOnStageSelect()
    {
        OnStageSelect.Invoke(step.StageScene);
    }
}
