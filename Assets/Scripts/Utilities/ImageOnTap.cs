﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

[RequireComponent(typeof(Image))]

public class ImageOnTap : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite onTapSprite;

    public UnityEvent OnTap = new UnityEvent();

    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
        ShowDefault();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Input.GetMouseButton(1)) return;
        ShowOnTap();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (Input.GetMouseButton(1)) return;
        ShowDefault();
    }

    private void ShowOnTap()
    {
        image.sprite = onTapSprite;
        OnTap.Invoke();
    }

    private void ShowDefault()
    {
        image.sprite = defaultSprite;
    }
}
