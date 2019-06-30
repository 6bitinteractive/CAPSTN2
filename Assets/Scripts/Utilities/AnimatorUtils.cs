using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorUtils : MonoBehaviour
{
    public static bool IsDonePlaying(Animator animator, string state, int layer = 0, float normalizedTime = 1f)
    {
        return animator.GetCurrentAnimatorStateInfo(layer).normalizedTime >= normalizedTime;
    }

    private static bool IsInState(Animator animator, string state, int layer = 0)
    {
        return animator.GetCurrentAnimatorStateInfo(layer).IsName(state);
    }
}
