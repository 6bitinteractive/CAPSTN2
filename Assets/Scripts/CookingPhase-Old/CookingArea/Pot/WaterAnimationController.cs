using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class WaterAnimationController : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SwitchState(WaterState waterState)
    {
        if (animator == null) { return; }
        animator.SetInteger("State", (int)waterState);
    }

    // NOTE: fuction overload since Unity doesn't support enum parameters at UnityEvent inspector :(
    public void SwitchState(int state)
    {
        if (animator == null) { return; }
        animator.SetInteger("State", state);
    }

    public enum WaterState
    {
        None, Pouring, Still, Simmering
    }
}
