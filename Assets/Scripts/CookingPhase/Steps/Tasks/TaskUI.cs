using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Duration))]
[RequireComponent(typeof(Animator))]

public class TaskUI : MonoBehaviour
{
    [SerializeField] private GameObject taskImageHidden;
    [SerializeField] private GameObject taskImageRevealed;
    [SerializeField] private Image timerBg;

    private Duration duration;
    private Animator animator;

    private void Awake()
    {
        duration = GetComponent<Duration>();
        animator = GetComponent<Animator>();
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

    public void Reveal()
    {
        animator.SetTrigger("Reveal");
    }

    public IEnumerator Hide()
    {
        animator.SetTrigger("Hide");
        yield return new WaitUntil(() => AnimatorUtils.IsInState(animator, "TaskHide"));
        yield return new WaitUntil(() => AnimatorUtils.IsDonePlaying(animator, "TaskHide"));
        gameObject.SetActive(false);
    }
}
