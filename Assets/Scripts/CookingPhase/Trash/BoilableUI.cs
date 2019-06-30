using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoilableUI : MonoBehaviour
{
    [Tooltip("This covers the lower/left area to show the right area for the slider to target")]
    [SerializeField] private Image startImage; // Covers the first section of the bar

    [Tooltip("This covers the upper/right area to show the right area for the slider to target")]
    [SerializeField] private Image endImage; // Covers the other end of the bar

    private Cookable boilableItem;

    private void Awake()
    {
        boilableItem = GetComponentInParent<Cookable>();
        startImage.fillAmount = boilableItem.MinBoil;
        endImage.fillAmount = 1f - boilableItem.MaxBoil;
    }
}
