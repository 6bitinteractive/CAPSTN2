using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable] public class TiltEvent : UnityEvent<TiltDirection> { }

public class TiltDetector : MonoBehaviour
{
    public TiltEvent OnTilt = new TiltEvent();

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
