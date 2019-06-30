using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StepList : MonoBehaviour
{
    private List<Button> stepButtons;
    private Animator animator;

    private void Awake()
    {
        stepButtons = new List<Button>();
        stepButtons.AddRange(GetComponentsInChildren<Button>());

        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        foreach (var stepButton in stepButtons)
            stepButton.onClick.AddListener(FadeOutPanel);
    }

    private void OnDisable()
    {
        foreach (var stepButton in stepButtons)
            stepButton.onClick.RemoveListener(FadeOutPanel);
    }

    public void FadeOutPanel()
    {
        animator.SetTrigger("FadeOut");
    }

    public void FadeInPanel()
    {
        animator.SetTrigger("FadeIn");
    }
}
