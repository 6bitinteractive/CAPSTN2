using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable] public class MarkerEvent : UnityEvent<MarkerController> { }

public abstract class MarkerController : MonoBehaviour
{
    public float CurrentNormalizedPosition { get; protected set; }
    public MarkerEvent OnMarkerStop = new MarkerEvent();

    protected RectTransform rectTransform;

    protected virtual void Awake()
    { }

    protected virtual void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    protected virtual void OnEnable()
    {
        ResetMarker();
    }

    protected virtual void OnDisable()
    { }

    protected abstract void Update();

    public abstract void Stop();
    public abstract void ResetMarker();
}
