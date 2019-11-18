using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Minceable : MonoBehaviour
{
    [SerializeField] private RuntimeAnimatorController[] animatorControllers;
    [SerializeField] private UnityEvent OnMince;

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

    public void Mince()
    {
        OnMince.Invoke();
    }

}
