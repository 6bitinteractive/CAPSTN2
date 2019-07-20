using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LinearMarkerController : MarkerController
{
    [SerializeField] private FloatRange durationRange;
    [SerializeField] private Image timer;
    [SerializeField] private RectTransform parent;
    [SerializeField] private SwipeAreaPrompt swipeArea;

    private float width; // Width of the parent object; i.e. how far is the end of the arrow?
    private float speed; // speed needed to reach the end with given duration
    private bool active;
    private Vector2 currentPosition;

    protected override void Awake()
    {
        base.Awake();
        width = parent.rect.width;
        //Debug.Log("Width: " + width);
        swipeArea.OnCorrectSwipe.AddListener(Stop);
    }

    protected override void Update()
    {
        if (!active)
            return;

        currentPosition.x += speed * Time.deltaTime;
        //Debug.Log("Current position X: " + currentPosition.x);

        CurrentNormalizedPosition = currentPosition.x / width;
        timer.fillAmount = CurrentNormalizedPosition;

        if (CurrentNormalizedPosition >= 1f)
        {
            Stop();
            return;
        }

        // We reposition after checking if it's already past 1 so that it'll never go beyond the width
        rectTransform.anchoredPosition = new Vector3(currentPosition.x, currentPosition.y, 0f);
    }

    public override void ResetMarker()
    {
        active = true;
        timer.fillAmount = 0;
        currentPosition = Vector2.zero;

        Duration = durationRange.RandomInRange;
        Duration = Duration < 1f ? Duration : Mathf.Floor(Duration); // to floor or not to floor...
        Debug.Log("Duration: " + Duration);
        speed = width / Duration;
        Debug.Log("Speed: " + speed);
    }

    public override void Stop()
    {
        active = false;
        Debug.Log("Normalized: " + CurrentNormalizedPosition);
        OnMarkerStop.Invoke(this);
    }
}
