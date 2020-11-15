using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueHintDisplay : MonoBehaviour
{
    [SerializeField] private float delay = 3.5f;

    public Image characterPortrait;
    public TextMeshProUGUI dialogueText;

    private Animator animator;
    public bool shown { get; private set; } = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Show(bool value = false)
    {
        if (shown) return;

        animator.SetTrigger("SlideIn");
        shown = true;
        Invoke("Hide", delay);
    }

    public void Hide()
    {
        if(shown)
            animator.SetTrigger("SlideOut");

        shown = false;
    }
}
