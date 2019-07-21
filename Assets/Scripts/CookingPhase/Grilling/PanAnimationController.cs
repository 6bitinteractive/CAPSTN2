using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanAnimationController : MonoBehaviour
{
    private SwipeDirectionListener swipeDirectionListener;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        swipeDirectionListener = SingletonManager.GetInstance<SwipeDirectionListener>();
        swipeDirectionListener.OnCorrectSwipe.AddListener(FlipPan);
    }

    private void OnDisable()
    {
        swipeDirectionListener.OnCorrectSwipe.RemoveListener(FlipPan);
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
