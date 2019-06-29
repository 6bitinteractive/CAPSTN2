using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Animator))]

public class FlameAnimationController : MonoBehaviour
{
    private Animator animator;
    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();
        animator = GetComponent<Animator>();

        // Turn off flame
        //image.enabled = false; // NOTE: Current art implementation has no "off" setting
    }

    public void UpdateFlame(HeatSetting heatSetting)
    {
        image.enabled = true;

        switch (heatSetting)
        {
            case HeatSetting.Low:
                animator.SetInteger("State", 1);
                break;
            case HeatSetting.Medium:
                animator.SetInteger("State", 2);
                break;
            case HeatSetting.High:
                animator.SetInteger("State", 3);
                break;
            default:
                animator.SetInteger("State", 0);
                //image.enabled = false;
                break;
        }
    }
}
