using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class QuickTimeEventButton : Prompt
{
    [SerializeField] private float timerDuration = 3f;

    private Image timerImage;
    private Button button;
    private float currentTime;

    protected override void Awake()
    {
        base.Awake();
        currentTime = timerDuration;
        timerImage = GetComponent<Image>();
        button = GetComponentInChildren<Button>();
        button.onClick.AddListener(SuccessfulInput);
    }

    protected void OnEnable()
    {
        ResetTimer();
    }

    private void Update()
    {
        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            Debug.Log("Failed");
            OnFailedInput.Invoke();
            Hide();
            return;
        }

        timerImage.fillAmount = Mathf.Clamp01(currentTime / timerDuration);
    }

    private void SuccessfulInput()
    {
        Debug.Log("Success");
        OnSuccessfulInput.Invoke();
        Hide();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    public void ResetTimer()
    {
        currentTime = timerDuration;
        timerImage.fillAmount = 1f;
    }
}
