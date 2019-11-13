using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable] public class TiltEvent : UnityEvent<TiltDirection> { }

public class TiltDetector : MonoBehaviour
{
    public TiltEvent OnTilt = new TiltEvent();
    private TiltType tiltType;
    [SerializeField] private float requiredToTilt = 0.8f;
    private Vector3 currentTilt;

    public TiltType TiltType { get => tiltType; set => tiltType = value; }

    private void Start()
    {
        Input.gyro.enabled = true;
    }

    private void Update()
    {
        #region Standalone Input
#if UNITY_STANDALONE_WIN
        if (Input.GetMouseButtonDown(0))
            OnTilt.Invoke(TiltDirection.Left);
        else if (Input.GetMouseButtonDown(1))
            OnTilt.Invoke(TiltDirection.Right);
        else if (Input.GetKeyDown(KeyCode.Space))
            OnTilt.Invoke(TiltDirection.Up);
        else if (Input.GetKeyDown(KeyCode.Tab))
            OnTilt.Invoke(TiltDirection.Down);
#endif
        #endregion

        #region Mobile Input
#if UNITY_ANDROID || UNITY_IOS

        currentTilt = Input.gyro.gravity;

        switch(tiltType)
        {
            case TiltType.Horizontal:
                if (currentTilt.x <= -requiredToTilt)
                {
                    Debug.Log("left");
                    OnTilt.Invoke(TiltDirection.Left);
                }

                else if (currentTilt.x >= requiredToTilt)
                {
                    Debug.Log("right");
                    OnTilt.Invoke(TiltDirection.Right);
                }
                break;

            case TiltType.Vertical:
                if (currentTilt.y <= -requiredToTilt)
                {
                    Debug.Log("up");
                    OnTilt.Invoke(TiltDirection.Up);
                }

                else if (currentTilt.y >= requiredToTilt)
                {
                    Debug.Log("down");
                    OnTilt.Invoke(TiltDirection.Down);
                }
                break;
        }
#endif
        #endregion
    }
}

public enum TiltDirection
{
    None,
    Up,
    Down,
    Left,
    Right
}

public enum TiltType
{
    Horizontal,
    Vertical
}
