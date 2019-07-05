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

    public Duration Duration { get; private set; }
    private Animator animator;
    private LayoutElement layoutElement;

    private void Awake()
    {
        Duration = GetComponent<Duration>();
        animator = GetComponent<Animator>();
        layoutElement = GetComponent<LayoutElement>();
    }

    private void OnEnable()
    {
        Duration.OnTimerUpdate.AddListener(UpdateTimerBg);
    }

    private void OnDisable()
    {
        Duration.OnTimerUpdate.RemoveListener(UpdateTimerBg);
    }

    private void UpdateTimerBg()
    {
        if (Duration.MaxDuration > 0)
            timerBg.fillAmount = Mathf.Clamp01(Duration.CurrentTime / Duration.MaxDuration);
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
        layoutElement.ignoreLayout = true;
    }
}
