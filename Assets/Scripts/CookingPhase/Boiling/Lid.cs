using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Lid : MonoBehaviour
{
    public UnityEvent OnLidOn = new UnityEvent();
    public UnityEvent OnLidOff = new UnityEvent();

    public void InvokeLidOn()
    {
        OnLidOn.Invoke();
    }

    public void InvokeOnLidOff()
    {
        OnLidOff.Invoke();
    }
}
