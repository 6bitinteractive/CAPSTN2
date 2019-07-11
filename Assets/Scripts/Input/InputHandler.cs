using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [HideInInspector] public SwipeDetector SwipeDetector;

    private void Awake()
    {
        SingletonManager.Register<InputHandler>(this);

        SwipeDetector = GetComponent<SwipeDetector>();
        SwipeDetector.enabled = false;
    }
}
