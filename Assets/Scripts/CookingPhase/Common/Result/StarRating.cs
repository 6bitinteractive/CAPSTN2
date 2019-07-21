using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]

public class StarRating : MonoBehaviour
{
    [SerializeField] private Sprite dim, highlight;

    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
        image.sprite = dim;
    }

    public void SwitchOn(bool turnOnHighlight = true)
    {
        image.sprite = turnOnHighlight ? highlight : dim;
    }
}
