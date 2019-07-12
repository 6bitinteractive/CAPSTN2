using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorUtils : MonoBehaviour
{
    // state = typically, it's the name of the clip
    public static bool IsDonePlaying(Animator animator, string state, int layer = 0, float percentage = 1f)
    {
        return animator.GetCurrentAnimatorStateInfo(layer).normalizedTime >= percentage;
    }

    public static bool IsInState(Animator animator, string state, int layer = 0)
    {
        return animator.GetCurrentAnimatorStateInfo(layer).IsName(state);
    }
}
