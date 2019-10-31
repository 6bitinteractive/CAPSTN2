using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minceable : MonoBehaviour
{
    [SerializeField] private RuntimeAnimatorController[] animatorControllers;

    private Animator animator;
    private int currentAnimatorController;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

   public void UpdateCurrentAnimatorController()
    {
        animator.runtimeAnimatorController = animatorControllers[currentAnimatorController++];
    }
}
