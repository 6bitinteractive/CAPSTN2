using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressMeter : MonoBehaviour
{
    [Range(0, 1)] public float perfectMin = 0.635f;
    [Range(0, 1)] public float perfectMax = 0.874f;

    public float PerfectMid => (perfectMin + perfectMax) * 0.5f;

    [SerializeField] private RectTransform progressIndicator;
    [SerializeField] private RectTransform progressMeterSections;
    public Image ProgressMeterImage { get; set; }

    public ProgressMeterState CurrentState { get; private set; }

    private List<ProgressMeterSection> sections = new List<ProgressMeterSection>();
    private float maxLength;
    private float offset;

    private void Awake()
    {
        ProgressMeterImage = progressMeterSections.GetComponent<Image>();

        // Get all sections
        sections.AddRange(progressMeterSections.GetComponentsInChildren<ProgressMeterSection>());

        // NOTE: Assumes progress bar is vertical
        maxLength = progressMeterSections.rect.height;
        offset = progressIndicator.anchoredPosition.y;
    }

    public void UpdateProgress(float value)
    {
        if (progressIndicator.anchoredPosition.y >= maxLength + offset)
            return;

        // Move indicator
        float y = maxLength * value;
        progressIndicator.anchoredPosition = new Vector2(progressIndicator.anchoredPosition.x, y + offset);

        // Test if indicator is in correct position
        // ProgressMeterImage.fillAmount = value;

        // TODO: determining CurrentState
    }
}

public enum ProgressMeterState
{
    Awful, Good, Great, Perfect
}
