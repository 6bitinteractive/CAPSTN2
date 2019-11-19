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
    [SerializeField] private GameObject[] starRatingImages;
    [SerializeField] private Sprite current, done, locked;

    [HideInInspector] public OnStageSelect OnStageSelect = new OnStageSelect();

    private Step step;
    private Image image;
    private Button button;

    private void Awake()
    {
        step = GetComponent<Step>();
        image = GetComponent<Image>();
        button = GetComponent<Button>();
        button.onClick.AddListener(InvokeOnStageSelect);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(InvokeOnStageSelect);
    }

    public void UpdateStepUI()
    {
        if (step.Locked)
        {
            button.enabled = false;
            image.sprite = locked;
        }
        else
        {
            button.enabled = true;
        }

        // If there's a rating for the step...
        if (step.Rating > 0)
        {
            // ...show same number of stars
            for (int i = 0; i < step.Rating; i++)
                starRatingImages[i].SetActive(true);

            // ...change image
            image.sprite = done;
            image.SetNativeSize();
        }
    }

    public void SetAsCurrent()
    {
        image.sprite = current;
        image.SetNativeSize();
    }

    private void InvokeOnStageSelect()
    {
        OnStageSelect.Invoke(step.StageScene);
    }
}
