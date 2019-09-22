using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]

public class Timeskip : MonoBehaviour, IPointerDownHandler
{
    private Image image;

    private void Awake()
    {
        SingletonManager.Register<Timeskip>(this);
        image = GetComponent<Image>();
        image.enabled = false;
    }

    private void OnDestroy()
    {
        SingletonManager.UnRegister<Timeskip>();
    }

    public void Show(Sprite sprite)
    {
        image.sprite = sprite;
        image.enabled = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        image.enabled = false;
    }
}
