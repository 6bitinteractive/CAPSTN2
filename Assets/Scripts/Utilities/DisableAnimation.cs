using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is a workaround to allow dragging of objects after their animation ends (eg. sliding in to scene) :(
// Animator conflicts with transform positions (will require to update in LateUpdate(), ...)
public class DisableAnimation : MonoBehaviour
{
    public Animator animator { get; private set; }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void EnableComponent(int enable)
    {
        animator.enabled = enable != 0;
    }
}

