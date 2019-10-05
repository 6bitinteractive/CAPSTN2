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

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Show(bool value = false)
    {
        animator.SetTrigger("SlideIn");
        Invoke("Hide", delay);
    }

    private void Hide()
    {
        animator.SetTrigger("SlideOut");
    }
}
