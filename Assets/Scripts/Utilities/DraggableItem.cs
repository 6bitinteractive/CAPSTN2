﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[SerializeField] public class OnItemDrop : UnityEvent<DraggableItem> { }

[RequireComponent(typeof(Collider2D))]

public class DraggableItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool Grabbed { get; private set; }
    public OnItemDrop OnDropItem = new OnItemDrop();

    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (Grabbed)
            Drag();
    }

    private void Drag()
    {
        #region Standard Input
#if UNITY_STANDALONE_WIN
        if (!rectTransform) // If this is not a UI element
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePosition.x, mousePosition.y, 0f); // set z-position to 0 to avoid it going "invisible" (being set to -10)
        }
        else
        {
            transform.position = Input.mousePosition;
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
                    if (!rectTransform)
                    {
                        Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                        transform.position = new Vector3(touchPosition.x, touchPosition.y, 0f);
                    }
                    else
                    {
                        transform.position = touch.position;
                    }
                    break;
            }
        }
#endif
        #endregion
    }

    private void OnMouseDown()
    {
        Grabbed = true;
    }

    private void OnMouseUp()
    {
        Grabbed = false;
        OnDropItem.Invoke(this);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Grabbed item: " + gameObject.name);
        Grabbed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Grabbed = false;
        OnDropItem.Invoke(this);
    }
}