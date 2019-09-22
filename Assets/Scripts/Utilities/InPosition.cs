using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Audio;

public class InPosition : MonoBehaviour
{
    public UnityEvent OnDestinationReached = new UnityEvent();

    public void InvokeOnDestinationReached()
    {
        OnDestinationReached.Invoke();
    }
}
