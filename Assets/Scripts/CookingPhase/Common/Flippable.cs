using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]

public class Flippable : MonoBehaviour
{
    public UnityEvent OnFlip = new UnityEvent();

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Flip()
    {
        animator.SetTrigger("Flip");
    }
}
