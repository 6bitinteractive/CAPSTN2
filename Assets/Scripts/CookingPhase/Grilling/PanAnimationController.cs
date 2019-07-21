using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanAnimationController : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void FlipPan(SwipeData swipeData)
    {
        switch (swipeData.Direction)
        {
            case SwipeDirection.Left:
                animator.SetTrigger("FlipLeft");
                break;
            case SwipeDirection.Right:
                animator.SetTrigger("FlipRight");
                break;
            case SwipeDirection.Up:
                animator.SetTrigger("FlipUp");
                break;
            case SwipeDirection.Down:
                animator.SetTrigger("FlipDown");
                break;
            default:
                Debug.LogError(gameObject.name + ": Not a valid direction");
                break;
        }
    }
}
