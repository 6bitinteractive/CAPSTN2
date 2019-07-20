using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotatingMarkerController : MarkerController
{
    [SerializeField] private FloatRange durationRange;
    [SerializeField] private float maxRotation = 360f;
    [SerializeField] private Button button;
    [SerializeField] private Image timer;

    private float angle; // angle needed to reach one rotation with given duration
    private float currentAngle;
    private bool spinning;

    protected override void Awake()
    {
        button.onClick.AddListener(Stop);
    }

    protected override void Update()
    {
        if (!spinning)
            return;

        // Clockwise
        currentAngle -= angle * Time.deltaTime;
        //currentAngle %= maxRotation;  // Keep the value within maxRotation
        //Debug.Log("Current rotation: " + currentAngle);

        // Rotate object
        rectTransform.rotation = Quaternion.Euler(0f, 0f, currentAngle);

        // Normalize
        CurrentNormalizedPosition = Mathf.Abs(currentAngle / maxRotation);
        timer.fillAmount = CurrentNormalizedPosition;

        // Player missed to press the button
        if (CurrentNormalizedPosition >= 1f)
        {
            Stop();
            return;
        }
    }

    public override void Stop()
    {
        spinning = false;
        Debug.Log("Normalized: " + CurrentNormalizedPosition);
        OnMarkerStop.Invoke(this);
    }

    public override void ResetMarker()
    {
        spinning = true;
        timer.fillAmount = 0;
        angle = currentAngle = 0f;

        Duration = durationRange.RandomInRange;
        Duration = Duration < 1f ? Duration : Mathf.Floor(Duration); // to floor or not to floor...
        Debug.Log("Duration: " + Duration);
        angle = maxRotation / Duration;
    }
}
