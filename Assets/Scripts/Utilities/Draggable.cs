﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[System.Serializable] public class OnItemDrop : UnityEvent<Draggable> { }

[RequireComponent(typeof(Collider2D))]

public class Draggable : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool Grabbed { get; private set; }
    [HideInInspector] public OnItemDrop OnDropItem = new OnItemDrop();

    private Canvas canvas; // Canvas that contains this draggable element
    private RectTransform rectTransform;

    private void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (Grabbed)
        {
            Drag();
        }
    }

    private void Drag()
    {
        #region Standard Input
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
        if (!rectTransform || canvas.renderMode == RenderMode.ScreenSpaceCamera) // If this is not a UI element or rendering using Screenspace - Camera
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePosition.x, mousePosition.y, 0f); // set z-position to 0 to avoid it going "invisible" (being set to -10)
        }
        else
        {
            transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
        }
#endif
        #endregion

        #region Mobile Input
#if UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                case TouchPhase.Moved:
                    if (!rectTransform || canvas.renderMode == RenderMode.ScreenSpaceCamera)
                    {
                        Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                        transform.position = new Vector3(touchPosition.x, touchPosition.y, 0f);
                    }
                    else
                    {
                        transform.position = new Vector3(touch.position.x, touch.position.y, 0f);
                    }
                    break;
            }
        }
#endif
        #endregion
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Input.GetMouseButton(1) && !Input.GetMouseButton(0)) return;

        Debug.Log("Grabbed item: " + gameObject.name);
        Grabbed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (Input.GetMouseButtonUp(1) && !Input.GetMouseButtonUp(0)) return;

        Grabbed = false;
        resetOnNoCollision();
        OnDropItem.Invoke(this);
    }

    private void resetOnNoCollision()
    {
        Collider2D col = this.GetComponent<Collider2D>();

        if (col == null || col.enabled == false) return;

        List<Collider2D> collidedWith = new List<Collider2D>();
        if (col == null) return;

        ContactFilter2D filter = new ContactFilter2D
        {
            useTriggers = true,
            useLayerMask = true,
            layerMask = LayerMask.GetMask("Cookware")
        };

        Physics2D.OverlapCollider(col, filter, collidedWith);

        if (collidedWith.Count == 0)
        {
            Debug.Log("Not colliding with anything");

            if (this.GetComponent<FoodPrepUtensil>() != null)
                this.GetComponent<FoodPrepUtensil>().Reset();

            if (this.GetComponent<Cookable>() != null)
                this.GetComponent<Cookable>().Reset();

            if (this.GetComponent<SpoonKitchenUtensil>() != null)
                this.GetComponent<SpoonKitchenUtensil>().Reset();
        }
    }
}
