using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public bool test;

    private void Awake()
    {
        SingletonManager.Register<InputHandler>(this);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            test = true;
            Debug.Log("t");
        }
    }
}
