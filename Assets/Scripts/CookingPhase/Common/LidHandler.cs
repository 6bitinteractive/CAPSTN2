﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Draggable))]

public class LidHandler : MonoBehaviour
{
    public bool IsCoveringCookware { get; private set; }

    private RectTransform rectTransform;
    private BoxCollider2D boxCollider;
    private Draggable draggable;
    private Cookware cookware;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        boxCollider = GetComponent<BoxCollider2D>();
        draggable = GetComponent<Draggable>();
    }

    private void OnEnable()
    {
        draggable.OnDropItem.AddListener(ClampPosition);
    }

    private void ClampPosition(Draggable draggedObject)
    {
        if (!IsCoveringCookware) { return; }

        rectTransform.position = cookware.LidPosition.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        cookware = collision.GetComponent<Cookware>();
        IsCoveringCookware = cookware;

        Debug.Log("Lid - Covering cookware");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        cookware = null;
        IsCoveringCookware = false;
        Debug.Log("Lid - off");
    }
}
