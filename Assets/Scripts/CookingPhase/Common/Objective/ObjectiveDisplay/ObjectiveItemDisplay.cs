using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ObjectiveItemDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI objectiveDescriptionText;
    [SerializeField] private Toggle toggle;
    [SerializeField] private Image toggleCheckmark;
    [SerializeField] Sprite check, cross;

    public Objective CorrespondingObjective { get; set; }

    public void SetDescriptionText(string value)
    {
        objectiveDescriptionText.text = value;
    }

    public void SetToggleCheckbox(bool successful)
    {
        toggleCheckmark.sprite = successful ? check : cross;
        toggle.isOn = true;
    }
}
