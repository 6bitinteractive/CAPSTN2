using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Duration))]

public class TaskUI : MonoBehaviour
{
    [SerializeField] private GameObject taskImageHidden;
    [SerializeField] private GameObject taskImageRevealed;
    [SerializeField] private Image timerBg;

    private Duration duration;

    private void Awake()
    {
        duration = GetComponent<Duration>();
    }

    private void OnEnable()
    {
        duration.OnTimerUpdate.AddListener(UpdateTimerBg);
    }

    private void OnDisable()
    {
        duration.OnTimerUpdate.RemoveListener(UpdateTimerBg);
    }

    private void UpdateTimerBg()
    {
        if (duration.MaxDuration > 0)
            timerBg.fillAmount = Mathf.Clamp01(duration.CurrentTime / duration.MaxDuration);
    }
}
