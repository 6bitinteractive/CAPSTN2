using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]

public class ImageOnTap : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite onTapSprite;

    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
        ShowDefault();
    }

    private void OnMouseDown()
    {
        ShowOnTap();
    }

    private void OnMouseUp()
    {
        ShowDefault();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        ShowOnTap();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ShowDefault();
    }

    private void ShowOnTap()
    {
        image.sprite = onTapSprite;
    }

    private void ShowDefault()
    {
        image.sprite = defaultSprite;
    }
}
