using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class SetIntAnimController : MonoBehaviour
{
    [SerializeField] private string parameterName = "State";
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetInt(int state)
    {
        animator.SetInteger(parameterName, state);
    }
}
