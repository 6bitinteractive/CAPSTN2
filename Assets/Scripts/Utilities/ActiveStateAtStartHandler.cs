using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveStateAtStartHandler : MonoBehaviour
{
    [SerializeField] private bool activeAtStart = true;

    private void Start()
    {
        gameObject.SetActive(activeAtStart);
    }
}
