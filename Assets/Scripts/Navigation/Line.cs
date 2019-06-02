using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    public int Index;
    public Transform Position;
    
    private bool isAvailable;

    public bool IsAvailable
    {
        get
        {
            return isAvailable;
        }

        set
        {
            isAvailable = value;
        }
    }
}
