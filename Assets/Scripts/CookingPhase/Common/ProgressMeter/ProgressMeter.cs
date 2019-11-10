using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressMeter : MonoBehaviour
{
    [Range(0, 1)]
    public float perfectMin = 0.635f;
    public float perfectMax = 0.874f;

    public float PerfectMid => (perfectMin + perfectMax) * 0.5f;

    [SerializeField] private Image progressIndicator;
    [SerializeField] private RectTransform progressMeterSections;
    public Image ProgressMeterImage { get; set; }

    public ProgressMeterState CurrentState { get; private set; }

    private List<ProgressMeterSection> sections = new List<ProgressMeterSection>();
    private float maxLength;

    private void Awake()
    {
        // Get all sections
        sections.AddRange(progressMeterSections.GetComponentsInChildren<ProgressMeterSection>());

        // NOTE: Assumes progress bar is vertical
        maxLength = progressMeterSections.rect.height;
        ProgressMeterImage = progressMeterSections.GetComponent<Image>();
    }

    public void UpdateProgress(float value)
    {
        // Move indicator
        //progressIndicator.transform.position += value * Vector3.up;
        ProgressMeterImage.fillAmount = value;
    }
}

public enum ProgressMeterState
{
    Awful, Good, Great, Perfect
}
