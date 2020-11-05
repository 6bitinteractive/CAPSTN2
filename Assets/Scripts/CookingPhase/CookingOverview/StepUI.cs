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
    private Vector2 initialSize;

    private void Awake()
    {
        step = GetComponent<Step>();
        image = GetComponent<Image>();
        button = GetComponent<Button>();
        button.onClick.AddListener(InvokeOnStageSelect);
        initialSize = image.rectTransform.sizeDelta;
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

    // 11/5 Added additional functions for selection

    public void SetAsShaded()
    {
        image.sprite = done;
        image.rectTransform.sizeDelta = initialSize;
    }

    public void ClickButton()
    {
        button.onClick.Invoke();
    }

    public SceneData GetSceneData()
    {
        return step.StageScene;
    }

    public bool IsLocked()
    {
        return step.Locked;
    }
}
