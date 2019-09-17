using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SwipeDetector))]

public class RemoveScumObjective : Objective
{
    [SerializeField] private WaterScum waterScum;
    [SerializeField] private KitchenUtensil ladle;

    private SwipeDetector swipeDetector;

    protected override void Awake()
    {
        base.Awake();
        swipeDetector = GetComponent<SwipeDetector>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        swipeDetector.enabled = true;
        SwipeDetector.OnSwipe += VerifySwipe;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        swipeDetector.enabled = false;
        SwipeDetector.OnSwipe -= VerifySwipe;
    }

    protected override void InitializeObjective()
    {
        base.InitializeObjective();
        ladle.gameObject.SetActive(true);
    }

    protected override void FinalizeObjective()
    {
        base.FinalizeObjective();
        ladle.gameObject.SetActive(false);
    }

    protected override bool SuccessConditionMet()
    {
        return waterScum.Removed;
    }

    private void VerifySwipe(SwipeData swipeData)
    {
        switch (swipeData.Direction)
        {
            case SwipeDirection.Left:
            case SwipeDirection.LeftUp:
            case SwipeDirection.LeftDown:
                if (ladle.InCookware)
                {
                    waterScum.Remove();
                }
                break;
            default:
                break;
        }
    }
}
