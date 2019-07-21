using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]

public class ResultText : MonoBehaviour
{
    [SerializeField] private Sprite[] resultTexts;

    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void DisplayTextRating(int rating)
    {
        image.sprite = resultTexts[rating - 1];
    }
}