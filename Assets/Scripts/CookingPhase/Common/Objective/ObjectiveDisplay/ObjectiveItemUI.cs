﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Animator))]

public class ObjectiveItemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI instructionText;
    [SerializeField] private Image checkMarkImage, checkMarkBgImage;
    [SerializeField] private Sprite checkMark, crossMark;
    [SerializeField] private Color successColor, failColor;

    public Objective CorrespondingObjective { get; set; }

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetData(string title, string instruction)
    {
        titleText.text = title;
        instructionText.text = instruction;
    }

    public void SetCheckmark(bool successful)
    {
        checkMarkImage.sprite = successful ? checkMark : crossMark;
        checkMarkBgImage.color = successful ? successColor : failColor;
        checkMarkImage.gameObject.SetActive(true);
        checkMarkBgImage.gameObject.SetActive(true);
    }

    public void Show(bool display = true)
    {
        if (display)
            animator.SetTrigger("SlideIn");
        else
            StartCoroutine(Hide());
    }

    private IEnumerator Hide()
    {
        yield return new WaitForSeconds(1f); // So that the player can see the checkmark change
        animator.SetTrigger("SlideOut");
        //yield return new WaitForSeconds(0.5f);
        transform.SetAsFirstSibling();
    }
}
