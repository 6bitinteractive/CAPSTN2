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
        animator.SetInteger("State", (int)heatSetting);

        //if (heatSetting == HeatSetting.Off)
        //    image.enabled = false; // NOTE: Current art implementation has no "off" setting
    }
}
