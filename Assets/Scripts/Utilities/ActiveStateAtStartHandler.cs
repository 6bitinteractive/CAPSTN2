using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveStateAtStartHandler : MonoBehaviour
{
    [SerializeField] private bool activeAtStart = true;

    private void Awake()
    {
        gameObject.SetActive(activeAtStart);
    }
}
